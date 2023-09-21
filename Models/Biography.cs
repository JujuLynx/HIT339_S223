namespace e_corp.Models
{
    public class Biography
    {
        // Keys
        public Guid BiographyID { get; set; }
        public string Content { get; set; }

        // Foreign Keys
        public string UserID { get; set; }
    }
}
