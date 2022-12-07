namespace Music.db.ViewModels.Genre
{
	using Music.db.Models;
	public class GenreDetailsViewModel
	{
		public string Name { get; set; }

		public List<Song> Songs { get; set; }
	}
}
