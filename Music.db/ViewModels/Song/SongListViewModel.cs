namespace Music.db.ViewModels.Song
{
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Music.db.Models;
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Linq;

    public class SongListViewModel
    {
        public string SongSearch { get; set; }
        public List<Song> Songs { get; set; }

        public int SongId { get; set; }
        public string Title { get; set; }
		[Display(Name = "Genre")]
		public int GenreId { get; set; }
        public string GenreName { get; set; }
    }
}
