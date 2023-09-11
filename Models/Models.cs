using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_corp.Models
{
    public class Session
    {
        // Keys
        public int SessionID { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }

        // Foreign Keys
        public string CoachID { get; set; }
        public int LocationID { get; set; }

        // Navigation Properties
        [ForeignKey("CoachID")]
        public IdentityUser Coach { get; set; }
        [ForeignKey("LocationID")]
        public Location Location { get; set; }
    }

    public class Booking
    {
        // Keys
        public int BookingID { get; set; }

        // Foreign Keys
        public int SessionID { get; set; }
        public int UserID { get; set; }

        // Navigation Properties
        [ForeignKey("SessionID")]
        public Session Session { get; set; }
        [ForeignKey("UserID")]
        public IdentityUser Member { get; set; }
    }

    public class Location
    {
        // Keys
        public int LocationID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }

    public class Biography
    {
        // Keys
        public int CoachID { get; set; }
        public string Content { get; set; }

        // Foreign Keys
        public int UserID { get; set; }
    }
}