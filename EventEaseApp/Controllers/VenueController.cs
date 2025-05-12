using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventEase.Models;
using Azure.Storage.Blobs;


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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Venue venue)
        {
            if (ModelState.IsValid)
            {
                if (venue.ImageFile != null)
                {
                    var blobUrl = await UploadImageToBlobAsync(venue.ImageFile);
                    venue.ImageUrl = blobUrl;
                }
                _context.Add(venue);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Venue successfully created!";
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
                    if (venue.ImageFile != null)
                    {
                        var blobUrl = await UploadImageToBlobAsync(venue.ImageFile);

                        venue.ImageUrl = blobUrl;
                    }
                    else
                    {

                    }
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
            var venue = await _context.Venue.FindAsync(id);
            if (venue == null) return NotFound();
            var hasAssociatedEvents = await _context.Event.AnyAsync(e => e.VenueID == id);
            if (hasAssociatedEvents)
            {
                TempData["ErrorMessage"] = "Cant delete this venue because there is a booking associated with it. ";
                return RedirectToAction(nameof(Index));
            }


            var hasBooking = await _context.Booking.AnyAsync(b => b.VenueID == id);
            if (hasBooking)
            {
                TempData["ErrorMessage"] = "Cant delete this venue because there is a booking associated with it. ";
                return RedirectToAction(nameof(Index));
            }
            _context.Venue.Remove(venue);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "The venue has been sucessfully deleted";
            return RedirectToAction(nameof(Index));
        }

        private async Task<string> UploadImageToBlobAsync(IFormFile imageFile)
        {
            // I have removed the connection string and container name and put them in the word document due to githubs policy violations

            var blobServiceClient = new BlobServiceClient(connectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(Guid.NewGuid() + Path.GetExtension(imageFile.FileName));

            var blobHttpHeaders = new Azure.Storage.Blobs.Models.BlobHttpHeaders
            {
                ContentType = imageFile.ContentType,
            };

            using (var stream = imageFile.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, new Azure.Storage.Blobs.Models.BlobUploadOptions
                {
                    HttpHeaders = blobHttpHeaders
                });
            }
            return blobClient.Uri.ToString();

        }

        private bool VenueExists(int id)
        {
            return _context.Venue.Any(e => e.VenueID == id);
        }
    }
}