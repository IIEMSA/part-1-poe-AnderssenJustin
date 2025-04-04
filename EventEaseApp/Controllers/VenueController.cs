using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventEase.Models;

namespace EventEase.Controllers
{
    public class VenueController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VenueController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Venue.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Venue venue)
        {
            if (ModelState.IsValid)
            {
                _context.Add(venue);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(venue);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var venue = await _context.Venue
                .FirstOrDefaultAsync(m => m.VenueID == id);

            if (venue == null) return NotFound();

            return View(venue);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var venue = await _context.Venue.FindAsync(id);
            if (venue == null) return NotFound();

            return View(venue);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Venue venue)
        {
            if (id != venue.VenueID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(venue);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VenueExists(venue.VenueID))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(venue);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var venue = await _context.Venue
                .FirstOrDefaultAsync(m => m.VenueID == id);

            if (venue == null) return NotFound();

            return View(venue);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var venue = await _context.Venue.FindAsync(id);
                if (venue == null)
                {
                    TempData["ErrorMessage"] = "Venue not found.";
                    return RedirectToAction(nameof(Index));
                }

                _context.Venue.Remove(venue);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException?.Message.Contains("FK_Booking_Venue") == true)
                {
                    TempData["ErrorMessage"] = "Cannot delete this venue because there are bookings associated with it.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Cannot delete this Venue as there is an event assosicated with it ";
                }
            }

            return RedirectToAction(nameof(Index));
        }


        private bool VenueExists(int id)
        {
            return _context.Venue.Any(e => e.VenueID == id);
        }
    }
}