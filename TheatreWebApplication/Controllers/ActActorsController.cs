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
    public class ActActorsController : Controller
    {
        private readonly TheatreContext _context;

        public ActActorsController(TheatreContext context)
        {
            _context = context;
        }

        // GET: ActActors
        public async Task<IActionResult> Index(int? id, int? flag) //flag = 0 -> по id акту, flag = 1 -> по id актора
        {
            if (flag == 0)
            {
                ViewBag.ActId = id;
                var actName = await _context.Acts.FirstOrDefaultAsync(a => a.Id == id);
                ViewBag.ActName = actName.ActName;
                var actors = _context.ActActors.Where(a => a.ActId == id).Include(a => a.Act).Include(a => a.Actor);
                return View(await actors.ToListAsync());
            }
            else if (flag == 1)
            {
                ViewBag.ActorId = id;
                var actorName = await _context.Actors.FirstOrDefaultAsync(a => a.Id == id);
                ViewBag.ActorName = actorName.ActorName;
                var acts = _context.ActActors.Where(a => a.ActorId == id).Include(a => a.Act).Include(a => a.Actor);
                return View(await acts.ToListAsync());
            }
            return View(await _context.ActActors.Include(a => a.Act).Include(a => a.Actor).ToListAsync());
        }

        // GET: ActActors/Details/5
        public async Task<IActionResult> Details(int? actid,int? actorid) // this method is redundant
        {
            if (actid == null || actorid == null|| _context.ActActors == null)
            {
                return NotFound();
            }

            var actActor = await _context.ActActors
                .Include(a => a.Act)
                .Include(a => a.Actor)
                .FirstOrDefaultAsync(m => m.ActId == actid && m.ActorId == actorid);
            if (actActor == null)
            {
                return NotFound();
            }

            return View(actActor);
        }

        // GET: ActActors/Create
        public IActionResult Create()
        {
            ViewData["ActId"] = new SelectList(_context.Acts, "Id", "ActName");
            ViewData["ActorId"] = new SelectList(_context.Actors, "Id", "ActorName");
            return View();
        }

        // POST: ActActors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ActId,ActorId,ContractDate")] ActActor actActor)
        {
            actActor.Act = await _context.Acts.FindAsync(actActor.ActId);
            actActor.Actor = await _context.Actors.FindAsync(actActor.ActorId);
            actActor.Act.Producer = await _context.Producers.FindAsync(actActor.Act.ProducerId);
            actActor.Act.Scenarist = await _context.Scenarists.FindAsync(actActor.Act.ScenaristId);
            ModelState.ClearValidationState(nameof(actActor.Act));
            ModelState.ClearValidationState(nameof(actActor.Actor));
            ModelState.ClearValidationState(nameof(actActor.Act.Producer));
            ModelState.ClearValidationState(nameof(actActor.Act.Scenarist));
            TryValidateModel(actActor.Act.Producer, nameof(actActor.Act.Producer));
            TryValidateModel(actActor.Act.Scenarist, nameof(actActor.Act.Scenarist));
            TryValidateModel(actActor.Actor, nameof(actActor.Actor));
            TryValidateModel(actActor.Act, nameof(actActor.Act));
            if (ModelState.IsValid)
            {
                _context.Add(actActor);
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex) { }
            }
            ViewData["ActId"] = new SelectList(_context.Acts, "Id", "ActName", actActor.ActId);
            ViewData["ActorId"] = new SelectList(_context.Actors, "Id", "ActorName", actActor.ActorId);
            return View(actActor);
        }

        // GET: ActActors/Edit/5
        public async Task<IActionResult> Edit(int? actid, int? actorid)
        {
            if (actid == null || actorid == null ||_context.ActActors == null)
            {
                return NotFound();
            }

            var actActor = await _context.ActActors.FindAsync(actid, actorid);
            if (actActor == null)
            {
                return NotFound();
            }
            //ViewData["ActId"] = new SelectList(_context.Acts, "Id", "ActName", actActor.ActId);
            //ViewData["ActorId"] = new SelectList(_context.Actors, "Id", "ActorName", actActor.ActorId);
            ViewBag.ActId = actid;
            ViewBag.ActorId = actorid;
            //ViewBag.ContractDate = _context.ActActors.Where().FirstOrDefault().ContractDate;
            return View(actActor);
        }

        // POST: ActActors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ActId,ActorId,ContractDate")] ActActor actActor)
        {
            /*if (id != actActor.ActId)
            {
                return NotFound();
            }*/

            actActor.Act = await _context.Acts.FindAsync(actActor.ActId);
            actActor.Actor = await _context.Actors.FindAsync(actActor.ActorId);
            actActor.Act.Producer = await _context.Producers.FindAsync(actActor.Act.ProducerId);
            actActor.Act.Scenarist = await _context.Scenarists.FindAsync(actActor.Act.ScenaristId);
            ModelState.ClearValidationState(nameof(actActor.Act));
            ModelState.ClearValidationState(nameof(actActor.Actor));
            ModelState.ClearValidationState(nameof(actActor.Act.Producer));
            ModelState.ClearValidationState(nameof(actActor.Act.Scenarist));
            TryValidateModel(actActor.Act.Producer, nameof(actActor.Act.Producer));
            TryValidateModel(actActor.Act.Scenarist, nameof(actActor.Act.Scenarist));
            TryValidateModel(actActor.Actor, nameof(actActor.Actor));
            TryValidateModel(actActor.Act, nameof(actActor.Act));
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(actActor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActActorExists(actActor.ActId))
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
            ViewData["ActId"] = new SelectList(_context.Acts, "Id", "ActName", actActor.ActId);
            ViewData["ActorId"] = new SelectList(_context.Actors, "Id", "ActorName", actActor.ActorId);
            return View(actActor);
        }

        // GET: ActActors/Delete/5
        public async Task<IActionResult> Delete(int? actid, int? actorid)
        {
            if (actid == null || actorid == null || _context.ActActors == null)
            {
                return NotFound();
            }

            var actActor = await _context.ActActors
                .Include(a => a.Act)
                .Include(a => a.Actor)
                .FirstOrDefaultAsync(m => m.ActId == actid && m.ActorId == actorid);
            if (actActor == null)
            {
                return NotFound();
            }

            return View(actActor);
        }

        // POST: ActActors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int actid, int actorid)
        {
            if (_context.ActActors == null)
            {
                return Problem("Entity set 'TheatreContext.ActActors'  is null.");
            }
            var actActor = await _context.ActActors.FindAsync(actid, actorid);
            if (actActor != null)
            {
                _context.ActActors.Remove(actActor);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActActorExists(int id)
        {
          return (_context.ActActors?.Any(e => e.ActId == id)).GetValueOrDefault();
        }
    }
}
