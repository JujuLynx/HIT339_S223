using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_corp.Models
{
    public class Booking
    {
        // Keys
        public Guid BookingID { get; set; }
        public Guid SessionID { get; set; }
        public string SessionName { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; }
        public string CoachName { get; set; }
        public string CoachEmail { get; set; }
        public string MemberID { get; set; }
        public string CoachID { get; set; }
        public string MemberEmail { get; set; }



    }
}
