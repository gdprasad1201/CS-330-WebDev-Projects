using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Fall2024_Assignment3_gdprasad.Data;
using Fall2024_Assignment3_gdprasad.Models;

namespace Fall2024_Assignment3_gdprasad.Controllers {
    public class MovieActorController : Controller {
        private readonly ApplicationDbContext _context;

        public MovieActorController(ApplicationDbContext context) {
            _context = context;
        }

        // GET: MovieActor
        public async Task<IActionResult> Index() {
            var applicationDbContext = _context.MovieActors.Include(m => m.Actor).Include(m => m.Movie);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: MovieActor/Details/5
        public async Task<IActionResult> Details(int? id) {
            if (id == null) {
                return NotFound();
            }

            var movieActor = await _context.MovieActors
                .Include(m => m.Actor)
                .Include(m => m.Movie)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movieActor == null) {
                return NotFound();
            }

            return View(movieActor);
        }

        // GET: MovieActor/Create
        public IActionResult Create() {
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Name");
            ViewData["ActorId"] = new SelectList(_context.Actors, "Id", "Name");
            return View();
        }

        // POST: MovieActor/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MovieId,ActorId")] MovieActor movieActor) {
            if (ModelState.IsValid) {
                _context.Add(movieActor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", "This movie-actor relationship already exists.");
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Name", movieActor.MovieId);
            ViewData["ActorId"] = new SelectList(_context.Actors, "Id", "Name", movieActor.ActorId);
            return View();
        }

        // GET: MovieActor/Edit/5
        public async Task<IActionResult> Edit(int? id) {
            if (id == null) {
                return NotFound();
            }

            var movieActor = await _context.MovieActors.FindAsync(id);
            if (movieActor == null) {
                return NotFound();
            }

            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Name", movieActor.MovieId);
            ViewData["ActorId"] = new SelectList(_context.Actors, "Id", "Name", movieActor.ActorId);
            
            return View(movieActor);
        }

        // POST: MovieActor/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MovieId,ActorId")] MovieActor movieActor) {
            if (id != movieActor.Id) {
                return NotFound();
            }
            
            if (ModelState.IsValid) {
                try {
                    _context.Update(movieActor);
                    await _context.SaveChangesAsync();
                } catch (DbUpdateConcurrencyException) {
                    if (!MovieActorExists(movieActor.Id)) {
                        return NotFound();
                    } else {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", "This movie-actor relationship already exists.");
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Name", movieActor.MovieId);
            ViewData["ActorId"] = new SelectList(_context.Actors, "Id", "Name", movieActor.ActorId);
            return View(movieActor);
        }

        // GET: MovieActor/Delete/5
        public async Task<IActionResult> Delete(int? id) {
            if (id == null) {
                return NotFound();
            }

            var movieActor = await _context.MovieActors
                .Include(m => m.Actor)
                .Include(m => m.Movie)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movieActor == null) {
                return NotFound();
            }

            return View(movieActor);
        }

        // POST: MovieActor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            var movieActor = await _context.MovieActors.FindAsync(id);
            if (movieActor != null) {
                _context.MovieActors.Remove(movieActor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieActorExists(int id) {
            return _context.MovieActors.Any(e => e.Id == id);
        }
    }
}
