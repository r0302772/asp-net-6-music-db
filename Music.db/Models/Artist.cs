namespace Music.db.Models
{
    public class Artist
    {
        public int Id { get; set; }
        public string Name { get; set; }

        //Navigation Props
        //public virtual ICollection<SongArtist> SongArtists { get; set; }
    }
}
