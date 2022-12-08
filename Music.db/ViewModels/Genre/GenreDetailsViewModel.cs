﻿namespace Music.db.ViewModels.Genre
{
	using Music.db.Models;
	public class GenreDetailsViewModel
	{
		public string Name { get; set; }

		public IEnumerable<Song> Songs { get; set; }
	}
}
