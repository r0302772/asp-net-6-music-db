using Microsoft.AspNetCore.Mvc.Rendering;

namespace Music.db.ViewModels.Song
{
	public class CreateSongViewModel
	{
		public string Title { get; set; }
		public int GenreId { get; set; }
		public SelectList? Genres { get; set; }//Setting this prop as nullable works but is not the right solution?
	}
}
