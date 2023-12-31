﻿namespace e_corp.Models
{
    public class SessionView
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; }
        public string CoachName { get; set; }
        public string CoachID { get; set; }
        public Guid SessionID { get; set; }
        public List<Booking> Bookings { get; set; }
    }
}
