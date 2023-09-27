namespace Music_Portal.Models
{
    public class Style
    {
        public Style()
        {
            this.Songs = new HashSet<Song>();
        }
        public int Id { get; set; }
        public string? Name { get; set; }
        public virtual ICollection<Song> Songs { get; set; }
    }
}
