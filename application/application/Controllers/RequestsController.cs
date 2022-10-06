using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using application.Data;
using application.Models;
using Microsoft.AspNetCore.Identity;
using DataAccess.Services;
using NuGet.Common;
using NuGet.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace application.Controllers
{
    public class RequestsController : Controller
    {
        private readonly SPaPSContext _context;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IEmailSenderEnhance emailService;

        public RequestsController(SPaPSContext context, UserManager<IdentityUser> userManager,
            IEmailSenderEnhance emailService)
        {
            this._context = context;
            this.userManager = userManager;
            this.emailService = emailService;
        }

        // GET: Requests
        public async Task<IActionResult> Index()
        {
            var sPaPSContext = _context.Requests.Include(r => r.Service);
            var refTypeId = _context.References.Where(x => x.ReferenceTypeId==4).ToList();

            List<String> buildingType = new List<String>();

            foreach (var request in sPaPSContext)
            {
                foreach (var item in refTypeId)
                {
                    if (item.ReferenceId == request.BuildingTypeId)
                    {
                        buildingType.Add(item.Description);
                    }
                }
            }
            ViewBag.br = buildingType.Count();
            ViewBag.BuildingTypeId = buildingType;

            return View(await sPaPSContext.ToListAsync());
        }

        // GET: Requests/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.Requests == null)
            {
                return NotFound();
            }

            var request = await _context.Requests
                .Include(r => r.Service)
                .FirstOrDefaultAsync(m => m.RequestId == id);
            if (request == null)
            {
                return NotFound();
            }

            return View(request);
        }

        // GET: Requests/Create
        [Authorize(Roles = "Корисник")]
        public IActionResult Create()
        {
            ViewData["Services"] = new SelectList(_context.Services.ToList(), "ServiceId", "Description");
            ViewData["BuildingTypes"] = new SelectList(_context.References
                .Where(x => x.ReferenceTypeId == 4).ToList(), "ReferenceId", "Description");
             
            Models.Request model = new Request()
            {
                RequestDate = DateTime.Now,
            };
            return View(model);
        }

        [Authorize(Roles = "Корисник")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Request request)
        {

            if (!ModelState.IsValid)
            {
                ViewData["Services"] = new SelectList(_context.Services.ToList(), "ServiceId", "Description");
                ViewData["BuildingTypes"] = new SelectList(_context.References
                    .Where(x => x.ReferenceTypeId == 4).ToList(), "ReferenceId", "Description");
                return View(request);
            }

            request.RequestDate = DateTime.Now;
            request.CreatedOn = DateTime.Now;
            request.CreatedBy = 1;
            request.IsActive = true;

            _context.Add(request);
            await _context.SaveChangesAsync();

            var service = _context.Services.Find(request.ServiceId);

            var serviceActivitiesIds = _context.ServiceActivities.Where(x => x.ServiceId == service.ServiceId)
                .Select(x=>x.ActivityId).ToList();


            var clientsIds = _context.ClientActivities.Where(x => serviceActivitiesIds.Contains(x.ActivityId))
                .Select(x=>x.ClientId)
                .Distinct()
                .ToList();

            foreach(var item in clientsIds)
            {
                var client = _context.Clients.Find(item);
                var user = await userManager.FindByIdAsync(client.UserId);

                if (await userManager.IsInRoleAsync(user, "Изведувач"))
                {

                    var token = await userManager.GeneratePasswordResetTokenAsync(user);

                    var callback = Url.Action(action: "Edit", controller: "Requests", values: new { id = request.RequestId }, HttpContext.Request.Scheme);

                    EmailSetUp emailSetUp = new EmailSetUp()
                    {
                        To = user.Email,
                        Template = "NewRequest",
                        Callback = callback,
                        RequestPath = emailService.PostalRequest(Request)
                    };

                    await emailService.SendEmailAsync(emailSetUp);
                }
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Requests/Edit/5
        [Authorize(Roles = "Изведувач")]
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.Requests == null)
            {
                return NotFound();
            }

            var request = await _context.Requests.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }
            ViewData["ServiceId"] = new SelectList(_context.Services, "ServiceId", "Description");

            ViewData["BuildingType"] = new SelectList(_context.References
                    .Where(x => x.ReferenceTypeId == 4).ToList(), "ReferenceId", "Description");

            var ids = request.ServiceId;

            ViewBag.ids = ids;

            return View(request);
        }

        // POST: Requests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Изведувач")]
        public async Task<IActionResult> Edit(long id,Request request)
        {
            if (id != request.RequestId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var updateRequest = _context.Requests.Find(request.RequestId);
                    if (updateRequest.ContractorId != null)
                    {
                        ViewData["ServiceId"] = new SelectList(_context.Services, "ServiceId", "Description");

                        ViewData["BuildingType"] = new SelectList(_context.References
                                .Where(x => x.ReferenceTypeId == 4).ToList(), "ReferenceId", "Description");

                        ModelState.AddModelError("Error", "Барањето со id " + updateRequest.RequestId + " е веќе прифатено од друг изведувач!");
                        return View(updateRequest);
                    }
                    var loggedInUserEmail = User.Identity.Name;

                    var user = await userManager.FindByEmailAsync(loggedInUserEmail);
                    var client = _context.Clients.Where(x => x.UserId == user.Id).FirstOrDefault();

                    updateRequest.ContractorId = client.ClientId;
                    updateRequest.UpdatedOn = DateTime.Now;
                    updateRequest.UpdatedBy = (int?)client.ClientId;

                    _context.Update(updateRequest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RequestExists(request.RequestId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ServiceId"] = new SelectList(_context.Services, "ServiceId", "ServiceId", request.ServiceId);
            return View(request);
        }

        // GET: Requests/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.Requests == null)
            {
                return NotFound();
            }

            var request = await _context.Requests
                .Include(r => r.Service)
                .FirstOrDefaultAsync(m => m.RequestId == id);
            if (request == null)
            {
                return NotFound();
            }

            return View(request);
        }

        // POST: Requests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.Requests == null)
            {
                return Problem("Entity set 'SPaPSContext.Requests'  is null.");
            }
            var request = await _context.Requests.FindAsync(id);
            if (request != null)
            {
                _context.Requests.Remove(request);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RequestExists(long id)
        {
          return _context.Requests.Any(e => e.RequestId == id);
        }
    }
}
