using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_corp.Models
{
    public class Session
    {
        // Keys
        public Guid SessionID { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; }
        public string CoachId { get; set; }


    }

}

