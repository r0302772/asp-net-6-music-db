using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Music.db.ViewModels.Song
{
	public class SongViewModel
	{
		public int SongId { get; set; }
		public string Title { get; set; }

		//Genre
		[Display(Name = "Genre")]
		public int GenreId { get; set; }
		public SelectList? Genres { get; set; }
		//Artist(s)
		[Display(Name = "Artist(s)")]
		public int[] ArtistsIds { get; set; }
		public MultiSelectList? Artists { get; set; }
		//Remixers
		[Display(Name = "Remixer")]
		public int? RemixerId { get; set; }
		public SelectList? Remixers { get; set; }
	}
}
