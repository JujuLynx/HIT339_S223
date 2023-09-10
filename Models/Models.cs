using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace e_corp.Models
{
	public class Coach
	{
        public int Id { get; set; }
        [Required]
        [StringLength(500)]
        public string Biography { get; set; }
        [Required]
        public IdentityUser User { get; set; }
    }

    public class Session
    {
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public IdentityUser User { get; set; }
        public Location Location { get; set; }
    }

    public class Booking
    {
        public int Id { get; set; }
        [Required]
        public Session Session { get; set; }
        [Required]
        public IdentityUser User { get; set; }
    }

    public class Location
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
    }
}

