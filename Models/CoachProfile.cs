using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace e_corp.Models
{
    public class CoachProfile
    {
        // Keys
        [Key]
        public Guid CoachID { get; set; }
        public string Name { get; set; }
        public int YearsOfExperience { get; set; }
        public string Biography { get; set; }
    }
}

