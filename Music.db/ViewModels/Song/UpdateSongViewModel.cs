using Microsoft.AspNetCore.Mvc.Rendering;
using Music.db.Models;

namespace Music.db.ViewModels.Song
{
	public class UpdateSongViewModel
	{
		public int SongId { get; set; }
		public string Title { get; set; }
		public int GenreId { get; set; }
		public int[] SelectedArtistIds { get; set; }
		public SelectList? Genres { get; set; }//Setting this prop as nullable works but is not the right solution?
		public MultiSelectList? Artists { get; set; }
	}
}
