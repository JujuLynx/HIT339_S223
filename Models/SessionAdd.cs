using Microsoft.AspNetCore.Identity;

namespace e_corp.Models
{
    public class SessionAdd
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; }

        public string CoachId { get; set; }
    }
}
