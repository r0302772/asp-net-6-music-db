﻿namespace Music.db.ViewModels.Artist
{
    using Music.db.Models;
    public class ArtistListViewModel
    {
        public string ArtistSearch { get; set; }
		public IEnumerable<Artist> Artists { get; set; }
    }
}
