namespace Music_Portal.Models
{
    public class Song
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Year { get; set; }
        public int? Style_id { get; set; }
        public virtual Style? Style { get; set; }
        public string? Clip { get; set; }
    }
}
