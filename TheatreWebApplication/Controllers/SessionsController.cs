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
    public class SessionsController : Controller
    {
        private readonly TheatreContext _context;

        public SessionsController(TheatreContext context)
        {
            _context = context;
        }

        // GET: Sessions
        public async Task<IActionResult> Index(int? id, string name)
        {
            if (id == null)
            {
                var theatreContext = _context.Sessions.Include(s => s.Act);
                return View(await theatreContext.ToListAsync());
            }
            else
            {
                ViewBag.ActId = id;
                ViewBag.ActName = name;
                var theatreContext = _context.Sessions.Where(s => s.ActId == id).Include(s => s.Act);
                return View(await theatreContext.ToListAsync());
            }
        }

        // GET: Sessions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Sessions == null)
            {
                return NotFound();
            }

            var session = await _context.Sessions
                .Include(s => s.Act)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (session == null)
            {
                return NotFound();
            }

            //return View(session);
            return RedirectToAction("Details", "Acts", new { id = session.ActId});
        }

        // GET: Sessions/Create
        public IActionResult Create()
        {
            ViewData["ActId"] = new SelectList(_context.Acts, "Id", "ActName");
            return View();
        }

        // POST: Sessions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ActId,DateAndTime")] Session session)
        {
            session.Act = await _context.Acts.FindAsync(session.ActId);
            session.Act.Producer = await _context.Producers.FindAsync(session.Act.ProducerId);
            session.Act.Scenarist = await _context.Scenarists.FindAsync(session.Act.ScenaristId);
            ModelState.ClearValidationState(nameof(session.Act));
            ModelState.ClearValidationState(nameof(session.Act.Producer));
            ModelState.ClearValidationState(nameof(session.Act.Scenarist));
            TryValidateModel(session.Act.Producer, nameof(session.Act.Producer));
            TryValidateModel(session.Act.Scenarist, nameof(session.Act.Scenarist));
            TryValidateModel(session.Act, nameof(session.Act));
            if (ModelState.IsValid)
            {
                _context.Add(session);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ActId"] = new SelectList(_context.Acts, "Id", "ActName");
            return View(session);
        }

        // GET: Sessions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Sessions == null)
            {
                return NotFound();
            }

            var session = await _context.Sessions.FindAsync(id);
            if (session == null)
            {
                return NotFound();
            }
            ViewData["ActId"] = new SelectList(_context.Acts, "Id", "ActName");
            return View(session);
        }

        // POST: Sessions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ActId,DateAndTime")] Session session)
        {
            if (id != session.Id)
            {
                return NotFound();
            }

            session.Act = await _context.Acts.FindAsync(session.ActId);
            session.Act.Producer = await _context.Producers.FindAsync(session.Act.ProducerId);
            session.Act.Scenarist = await _context.Scenarists.FindAsync(session.Act.ScenaristId);
            ModelState.ClearValidationState(nameof(session.Act));
            ModelState.ClearValidationState(nameof(session.Act.Producer));
            ModelState.ClearValidationState(nameof(session.Act.Scenarist));
            TryValidateModel(session.Act.Producer, nameof(session.Act.Producer));
            TryValidateModel(session.Act.Scenarist, nameof(session.Act.Scenarist));
            TryValidateModel(session.Act, nameof(session.Act));
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(session);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SessionExists(session.Id))
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
            ViewData["ActId"] = new SelectList(_context.Acts, "Id", "ActName");
            return View(session);
        }

        // GET: Sessions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Sessions == null)
            {
                return NotFound();
            }

            var session = await _context.Sessions
                .Include(s => s.Act)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (session == null)
            {
                return NotFound();
            }

            return View(session);
        }

        // POST: Sessions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Sessions == null)
            {
                return Problem("Entity set 'TheatreContext.Sessions'  is null.");
            }
            var session = await _context.Sessions.FindAsync(id);
            if (session != null)
            {
                _context.Sessions.Remove(session);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SessionExists(int id)
        {
          return (_context.Sessions?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
