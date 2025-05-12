using System.ComponentModel.DataAnnotations.Schema;

namespace EventEase.Models
{
    public class Venue
    {
        public int VenueID { get; set; }
        public string VenueName { get; set; }
        public string Location { get; set; }
        public int Capacity { get; set; }
        public string? ImageUrl { get; set; }

        [NotMapped]

        public IFormFile? ImageFile { get; set; }
        

    
        public List<Booking> Booking { get; set; } = new();
    }
}
