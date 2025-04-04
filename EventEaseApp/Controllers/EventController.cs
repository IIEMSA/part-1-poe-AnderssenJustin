using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventEase.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;

namespace EventEase.Controllers
{
    public class EventController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventController(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // GET: Event
        public async Task<IActionResult> Index()
        {
            var events = await _context.Event.Include(e => e.Venue).ToListAsync();
            return View(events);
        }

        // GET: Event/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Event
                .FirstOrDefaultAsync(m => m.EventId == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // GET: Event/Create
        public IActionResult Create()
        {
            var venue = _context.Venue.ToList();

            if (!venue.Any())
            {
                ViewBag.VenueID = new SelectList(new List<Venue>());
                ModelState.AddModelError("", "No venues available. Please add venues before creating an event.");
            }
            else
            {
                ViewBag.VenueID = new SelectList(venue, "VenueID", "VenueName");
            }

            return View();
        }

        // POST: Event/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EventName, EventDate, Description, VenueID")] Event @event)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@event);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // If the model state is invalid, return to the view with the venues list and validation errors
            var venue = _context.Venue.ToList();
            ViewBag.VenueID = new SelectList(venue, "VenueID", "VenueName", @event.VenueID);

            return View(@event);
        }

        // GET: Event/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Event.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }

            
            ViewData["VenueList"] = new SelectList(_context.Venue, "VenueID", "VenueName", @event.VenueID);

            return View(@event);
        }


        // POST: Event/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EventId, EventName, EventDate, Description, VenueID")] Event @event)
        {
            if (id != @event.EventId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@event);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(@event.EventId))
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
            return View(@event);
        }

        // GET: Event/Delete/5
        // GET: Event/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Event
                .Include(e => e.Venue) // Include venue if you want to display its details
                .FirstOrDefaultAsync(m => m.EventId == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // POST: Event/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @event = await _context.Event.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }

            try
            {
                _context.Event.Remove(@event);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                // Log the exception (ex) here
                TempData["ErrorMessage"] = "An error occurred while deleting the event. It may be referenced by other records.";
                return RedirectToAction(nameof(Index));
            }
        }



        private bool EventExists(int id)
        {
            return _context.Event.Any(e => e.EventId == id);
        }
    }
}

