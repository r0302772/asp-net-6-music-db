namespace Music.db.ViewModels.Song
{
	using Music.db.Models;
	public class DeleteSongViewModel
	{
		public int SongId { get; set; }
		public string Title { get; set; }

		public List<Song> Songs { get; set; }
	}
}
