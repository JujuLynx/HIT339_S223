using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_corp.Models
{
    public class Booking
    {
        // Keys
        public Guid BookingID { get; set; }

        // Foreign Keys
        public Guid SessionID { get; set; }
        public string UserID { get; set; }

        // Navigation Properties
        [ForeignKey("SessionID")]
        public Session Session { get; set; }
        [ForeignKey("UserID")]
        public IdentityUser Member { get; set; }
    }
}
