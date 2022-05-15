using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TheatreWebApplication;

namespace TheatreWebApplication.Controllers
{
    public class ScenaristsController : Controller
    {
        private readonly TheatreContext _context;

        public ScenaristsController(TheatreContext context)
        {
            _context = context;
        }

        // GET: Scenarists
        public async Task<IActionResult> Index()
        {
            return View(await _context.Scenarists.ToListAsync());
        }

        // GET: Scenarists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Scenarists == null)
            {
                return NotFound();
            }

            var scenarist = await _context.Scenarists
                .FirstOrDefaultAsync(m => m.Id == id);
            if (scenarist == null)
            {
                return NotFound();
            }

            //return View(scenarist);
            return RedirectToAction("Index", "Acts", new { id = scenarist.Id, name = scenarist.ScenaristName, flag = 1 });
        }

        // GET: Scenarists/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Scenarists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ScenaristName")] Scenarist scenarist)
        {
            if (ModelState.IsValid)
            {
                _context.Add(scenarist);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(scenarist);
        }

        // GET: Scenarists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Scenarists == null)
            {
                return NotFound();
            }

            var scenarist = await _context.Scenarists.FindAsync(id);
            if (scenarist == null)
            {
                return NotFound();
            }
            return View(scenarist);
        }

        // POST: Scenarists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ScenaristName")] Scenarist scenarist)
        {
            if (id != scenarist.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(scenarist);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScenaristExists(scenarist.Id))
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
            return View(scenarist);
        }

        // GET: Scenarists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Scenarists == null)
            {
                return NotFound();
            }

            var scenarist = await _context.Scenarists
                .Include(s => s.Acts)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (scenarist == null)
            {
                return NotFound();
            }

            return View(scenarist);
        }

        // POST: Scenarists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Scenarists == null)
            {
                return Problem("Entity set 'TheatreContext.Scenarists'  is null.");
            }
            var scenarist = await _context.Scenarists.FindAsync(id);
            if (scenarist != null)
            {
                _context.Scenarists.Remove(scenarist);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScenaristExists(int id)
        {
          return (_context.Scenarists?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
