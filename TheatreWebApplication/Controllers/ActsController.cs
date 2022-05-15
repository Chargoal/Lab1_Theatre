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
    public class ActsController : Controller
    {
        private readonly TheatreContext _context;

        public ActsController(TheatreContext context)
        {
            _context = context;
        }

        // GET: Acts
        public async Task<IActionResult> Index(int? id, string name, int? flag) //flag = 0 -> producer, flag = 1 -> scenarist
        {
            if (flag == 0)
            {
                ViewBag.ProducerId = id;
                ViewBag.ProducerName = name;
                var theatreContext = _context.Acts.Where(a => a.ProducerId == id).Include(a => a.Producer).Include(a => a.Scenarist);
                return View(await theatreContext.ToListAsync());
            }
            else if (flag == 1)
            {
                ViewBag.ScenaristId = id;
                ViewBag.ScenaristName = name;
                var theatreContext = _context.Acts.Where(a => a.ScenaristId == id).Include(a => a.Producer).Include(a => a.Scenarist);
                return View(await theatreContext.ToListAsync());
            }
            return View(await _context.Acts.Include(a => a.Producer).Include(a => a.Scenarist).ToListAsync());
        }

        // GET: Acts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Acts == null)
            {
                return NotFound();
            }

            var act = await _context.Acts
                .Include(a => a.Producer)
                .Include(a => a.Scenarist)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (act == null)
            {
                return NotFound();
            }

            return View(act);
        }

        // GET: Acts/Create
        public IActionResult Create()
        {
            ViewData["ProducerId"] = new SelectList(_context.Producers, "Id", "ProducerName");
            ViewData["ScenaristId"] = new SelectList(_context.Scenarists, "Id", "ScenaristName");
            return View();
        }

        // POST: Acts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string budget, [Bind("Id,ActName,Budget,Duration,ProducerId,ScenaristId")] Act act)
        {
            act.Budget = decimal.Parse(budget.Replace('.', ','));
            ModelState.ClearValidationState(nameof(act.Budget));
            TryValidateModel(act.Budget, nameof(act.Budget));
            act.Producer = await _context.Producers.FindAsync(act.ProducerId);
            ModelState.ClearValidationState(nameof(act.Producer));
            TryValidateModel(act.Producer, nameof(act.Producer));
            act.Scenarist = await _context.Scenarists.FindAsync(act.ScenaristId);
            ModelState.ClearValidationState(nameof(act.Scenarist));
            TryValidateModel(act.Scenarist, nameof(act.Scenarist));
            if (ModelState.IsValid)
            {
                _context.Add(act);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProducerId"] = new SelectList(_context.Producers, "Id", "ProducerName", act.ProducerId);
            ViewData["ScenaristId"] = new SelectList(_context.Scenarists, "Id", "ScenaristName", act.ScenaristId);
            return View(act);
        }

        // GET: Acts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Acts == null)
            {
                return NotFound();
            }

            var act = await _context.Acts.FindAsync(id);
            ViewBag.Budget=act.Budget;
            if (act == null)
            {
                return NotFound();
            }
            ViewData["ProducerId"] = new SelectList(_context.Producers, "Id", "ProducerName", act.ProducerId);
            ViewData["ScenaristId"] = new SelectList(_context.Scenarists, "Id", "ScenaristName", act.ScenaristId);
            return View(act);
        }

        // POST: Acts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string budget, [Bind("Id,ActName,Budget,Duration,ProducerId,ScenaristId")] Act act)
        {
            if (id != act.Id)
            {
                return NotFound();
            }

            act.Budget = decimal.Parse(budget.Replace('.', ','));
            act.Scenarist = await _context.Scenarists.FindAsync(act.ScenaristId);
            act.Producer = await _context.Producers.FindAsync(act.ProducerId);
            ModelState.ClearValidationState(nameof(act.Budget));
            ModelState.ClearValidationState(nameof(act.Producer));
            ModelState.ClearValidationState(nameof(act.Scenarist));
            TryValidateModel(act.Budget, nameof(act.Budget));
            TryValidateModel(act.Producer, nameof(act.Producer));
            TryValidateModel(act.Scenarist, nameof(act.Scenarist));
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(act);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActExists(act.Id))
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
            ViewData["ProducerId"] = new SelectList(_context.Producers, "Id", "ProducerName", act.ProducerId);
            ViewData["ScenaristId"] = new SelectList(_context.Scenarists, "Id", "ScenaristName", act.ScenaristId);
            return View(act);
        }

        // GET: Acts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Acts == null)
            {
                return NotFound();
            }

            var act = await _context.Acts
                .Include(a => a.Producer)
                .Include(a => a.Scenarist)
                .Include(a => a.ActActors)
                .Include(a => a.Sessions)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (act == null)
            {
                return NotFound();
            }

            return View(act);
        }

        // POST: Acts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Acts == null)
            {
                return Problem("Entity set 'TheatreContext.Acts'  is null.");
            }
            var act = await _context.Acts.FindAsync(id);
            if (act != null)
            {
                _context.Acts.Remove(act);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActExists(int id)
        {
          return (_context.Acts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
