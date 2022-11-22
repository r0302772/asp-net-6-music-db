namespace Music.db.ViewModels.Song
{
    using Music.db.Models;
    public class SongListViewModel
    {
        public string SongSearch { get; set; }
        public List<Song> Songs { get; set; }
    }
}
