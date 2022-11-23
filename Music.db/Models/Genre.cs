namespace Music.db.Models
{
    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; }

        //Navigation Props
        public ICollection<Song> Songs { get; set; }
    }
}
