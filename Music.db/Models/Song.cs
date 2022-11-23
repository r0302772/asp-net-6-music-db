namespace Music.db.Models
{
    public class Song
    {
        public int Id { get; set; }
        public string Title { get; set; }

        //Navigation Props
        public int GenreId { get; set; }
        public Genre Genre { get; set; }
        public virtual ICollection<SongArtist> SongArtists { get; set; }
    }
}
