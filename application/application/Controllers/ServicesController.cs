using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using application.Data;
using application.Models;
using Microsoft.AspNetCore.Authorization;
using application.Models.CustomModels;

namespace application.Controllers
{
    [Authorize(Roles = "Админ")] 
    public class ServicesController : Controller
    {
        private readonly SPaPSContext _context;

        public ServicesController(SPaPSContext context)
        {
            _context = context;
        }

        // GET: Services
        public async Task<IActionResult> Index()
        {
            List<vm_Service> services = await _context.Services
                                        .Include(x => x.ServiceActivities)
                                        .Select(x => new vm_Service()
                                         {
                                             ServiceId = x.ServiceId,
                                             Description = x.Description,
                                             Activities = String.Join("; ",x.ServiceActivities.Select(a => a.Activity.Name))
                                          }).ToListAsync();
            return View(services);
        }

        // GET: Services/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.Services == null)
            {
                return NotFound();
            }

            var service = await _context.Services
                .FirstOrDefaultAsync(m => m.ServiceId == id);
            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        // GET: Services/Create
        public IActionResult Create()
        {
            ViewBag.activities = new SelectList(_context.Activities.ToList(), "ActivityId", "Name");
            return View();
        }

        // POST: Services/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(vm_Service model)
        {
            if (ModelState.IsValid)
            {
                if (model.ActivityIds[0] != null)
                {
                    Service service = new Service()
                    {
                        Description = model.Description,
                        CreatedOn = DateTime.Now,
                        CreatedBy = 1
                    };
                    _context.Services.Add(service);
                    await _context.SaveChangesAsync();

                    List<ServiceActivity> serviceActivities = model.ActivityIds.Select(x => new ServiceActivity()
                    {
                        ServiceId = service.ServiceId,
                        ActivityId = (int)x
                    }).ToList();

                    await _context.ServiceActivities.AddRangeAsync(serviceActivities);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("Error", "Внесете активност!");
                    ViewBag.activities = new SelectList(_context.Activities.ToList(), "ActivityId", "Name");
                    return View(model);
                }
            }
            ViewBag.activities = new SelectList(_context.Activities.ToList(), "ActivityId", "Name");
            return View(model);
        }

        // GET: Services/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.Services == null)
            {
                return NotFound();
            }

            var service= await _context.Services.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }
            List<int?> ids = new List<int?>();
            foreach(var item in _context.ServiceActivities)
            {
                if (item.ServiceId.Equals(service.ServiceId))
                {
                    ids.Add(Convert.ToInt32(item.ActivityId));
                }
            }
            var model = new vm_ServiceChange()
            {
                Description = service.Description,
                ServiceId = service.ServiceId,
                ActivityIds = ids
            };
            ViewBag.activities = new SelectList(_context.Activities.ToList(), "ActivityId", "Name");
            return View(model);
        }

        // POST: Services/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, vm_ServiceChange model)
            {
            if (id != model.ServiceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (model.ActivityIds[0] != null)
                {
                    var service = await _context.Services.FindAsync(id);
                    service.Description = model.Description;
                    service.UpdatedOn = DateTime.Now;
                    service.UpdatedBy = 1;

                    _context.Services.Update(service);
                    await _context.SaveChangesAsync();

                    var servis = _context.ServiceActivities.Where(x => x.ServiceId.Equals(service.ServiceId));

                    _context.ServiceActivities.RemoveRange(servis);
                    await _context.SaveChangesAsync();
                    List<ServiceActivity> serviceActivities = new List<ServiceActivity>();
                    foreach (var item in model.ActivityIds)
                    {
                        ServiceActivity sa = new ServiceActivity()
                        {
                            ServiceId = service.ServiceId,
                            ActivityId = (long)item
                        };

                        serviceActivities.Add(sa);
                    }

                    _context.ServiceActivities.AddRange(serviceActivities);
                    await _context.SaveChangesAsync();


                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("Error", "Внесете активност!");
                    ViewBag.activities = new SelectList(_context.Activities.ToList(), "ActivityId", "Name");
                    return View(model);
                }
            }
            ViewBag.activities = new SelectList(_context.Activities.ToList(), "ActivityId", "Name");
            return View(model);
        }

        // GET: Services/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.Services == null)
            {
                return NotFound();
            }

            var service = await _context.Services
                .FirstOrDefaultAsync(m => m.ServiceId == id);
            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        // POST: Services/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.Services == null)
            {
                return Problem("Entity set 'SPaPSContext.Services'  is null.");
            }
            var service = await _context.Services.FindAsync(id);
            if (service != null)
            {
                _context.Services.Remove(service);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServiceExists(long id)
        {
          return _context.Services.Any(e => e.ServiceId == id);
        }
    }
}
