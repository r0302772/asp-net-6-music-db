namespace Music.db.ViewModels.Song
{
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Music.db.Models;
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Linq;

    public class SongListViewModel
    {
        public string SongSearch { get; set; }
        public IEnumerable<Song> Songs { get; set; }
        public IEnumerable<SongArtist> SongArtists { get; set; }
    }
}
