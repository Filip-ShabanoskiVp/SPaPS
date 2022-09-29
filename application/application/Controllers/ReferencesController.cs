using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using application.Data;
using application.Models;
using SPaPSContext = application.Data.SPaPSContext;

namespace application.Controllers
{
    public class ReferencesController : Controller
    {
        private readonly SPaPSContext _context;

        public ReferencesController(SPaPSContext context)
        {
            _context = context;
        }

        // GET: References
        public async Task<IActionResult> Index()
        {
            var sPaPSContext = _context.References.Include(r => r.ReferenceType);
            return View(await sPaPSContext.ToListAsync());
        }

        // GET: References/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.References == null)
            {
                return NotFound();
            }

            var reference = await _context.References
                .Include(r => r.ReferenceType)
                .FirstOrDefaultAsync(m => m.ReferenceId == id);
            if (reference == null)
            {
                return NotFound();
            }

            return View(reference);
        }

        // GET: References/Create
        public IActionResult Create()
        {
            ViewData["ReferenceTypes"] = new SelectList(_context.ReferenceTypes.ToList(), "ReferenceTypeId", "Description");
            return View();
        }

        // POST: References/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReferenceId,ReferenceTypeId,Description,Code,CreatedOn,CreatedBy,UpdatedOn,UpdatedBy,IsActive")] Reference reference)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reference);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ReferenceTypeId"] = new SelectList(_context.ReferenceTypes, "ReferenceTypeId", "ReferenceTypeId", reference.ReferenceTypeId);
            return View(reference);
        }

        // GET: References/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.References == null)
            {
                return NotFound();
            }

            var reference = await _context.References.FindAsync(id);
            if (reference == null)
            {
                return NotFound();
            }
            ViewData["ReferenceTypeId"] = new SelectList(_context.ReferenceTypes, "ReferenceTypeId", "ReferenceTypeId", reference.ReferenceTypeId);
            return View(reference);
        }

        // POST: References/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ReferenceId,ReferenceTypeId,Description,Code,CreatedOn,CreatedBy,UpdatedOn,UpdatedBy,IsActive")] Reference reference)
        {
            if (id != reference.ReferenceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reference);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReferenceExists(reference.ReferenceId))
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
            ViewData["ReferenceTypeId"] = new SelectList(_context.ReferenceTypes, "ReferenceTypeId", "ReferenceTypeId", reference.ReferenceTypeId);
            return View(reference);
        }

        // GET: References/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.References == null)
            {
                return NotFound();
            }

            var reference = await _context.References
                .Include(r => r.ReferenceType)
                .FirstOrDefaultAsync(m => m.ReferenceId == id);
            if (reference == null)
            {
                return NotFound();
            }

            return View(reference);
        }

        // POST: References/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.References == null)
            {
                return Problem("Entity set 'SPaPSContext.References'  is null.");
            }
            var reference = await _context.References.FindAsync(id);
            if (reference != null)
            {
                _context.References.Remove(reference);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReferenceExists(long id)
        {
          return (_context.References?.Any(e => e.ReferenceId == id)).GetValueOrDefault();
        }
    }
}
