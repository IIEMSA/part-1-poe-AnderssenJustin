using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using EventEase.Models;

namespace EventWebApp.Controllers
{
    public class EventController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventController(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index(int? eventTypeId, int? venueId, DateTime? startDate, DateTime? endDate)
        {
            var events = _context.Event
                .Include(e => e.Venue)
                .Include(e => e.EventType)
                .AsQueryable();

            if (eventTypeId.HasValue)
                events = events.Where(e => e.EventTypeID == eventTypeId.Value);

            if (venueId.HasValue)
                events = events.Where(e => e.VenueID == venueId.Value);

            if (startDate.HasValue)
                events = events.Where(e => e.EventDate.Date >= startDate.Value.Date);

            if (endDate.HasValue)
                events = events.Where(e => e.EventDate.Date <= endDate.Value.Date);

            
            ViewBag.EventTypes = await _context.EventType.ToListAsync();
            ViewBag.Venues = await _context.Venue.ToListAsync();
            ViewBag.SelectedEventTypeId = eventTypeId;
            ViewBag.SelectedVenueId = venueId;
            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;

            return View(await events.ToListAsync());
        }

        
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var eventModel = await _context.Event
                .Include(e => e.Venue)
                .Include(e => e.EventType)
                .FirstOrDefaultAsync(m => m.EventID == id);

            if (eventModel == null)
                return NotFound();

            return View(eventModel);
        }

        
        public IActionResult Create()
        {
            ViewData["VenueList"] = new SelectList(_context.Venue, "VenueID", "VenueName");
            ViewData["EventTypes"] = _context.EventType.ToList();
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EventID,EventName,EventDate,Description,VenueID,EventTypeID")] Event eventModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(eventModel);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Event created successfully!";
                return RedirectToAction(nameof(Index));
            }

            ViewData["VenueList"] = new SelectList(_context.Venue, "VenueID", "VenueName");
            ViewData["EventTypes"] = _context.EventType.ToList();
            return View(eventModel);
        }

        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var eventModel = await _context.Event.FindAsync(id);
            if (eventModel == null)
                return NotFound();

            ViewData["VenueList"] = new SelectList(_context.Venue, "VenueID", "VenueName");
            ViewData["EventTypes"] = new SelectList(_context.EventType, "EventTypeID", "Name");
            return View(eventModel);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EventID,EventName,EventDate,Description,VenueID,EventTypeID")] Event eventModel)
        {
            if (id != eventModel.EventID)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eventModel);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Event updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(eventModel.EventID))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["VenueList"] = new SelectList(_context.Venue, "VenueID", "VenueName");
            ViewData["EventTypes"] = new SelectList(_context.EventType, "EventTypeID", "Name");
            return View(eventModel);
        }

        
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var eventModel = await _context.Event
                .Include(e => e.Venue)
                .Include(e => e.EventType)
                .FirstOrDefaultAsync(m => m.EventID == id);

            if (eventModel == null)
                return NotFound();

            return View(eventModel);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eventModel = await _context.Event.FindAsync(id);
            if (eventModel != null)
            {
                _context.Event.Remove(eventModel);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Event deleted successfully!";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int id)
        {
            return _context.Event.Any(e => e.EventID == id);
        }
    }
}

