using Music.db.Models;

namespace Music.db.Data
{
	public static class DbInitializer
	{
		public static void Initialize(MusicdbContext context)
		{
			context.Database.EnsureCreated();

			if (context.Artists.Any()) return;
			if (context.Genres.Any()) return;
			if (context.Songs.Any()) return;
			if (context.SongArtists.Any()) return;

			#region Seed Artists

			var artists = new Artist[]
			{
				new Artist{Name="Elderbrook", },
				new Artist{Name="Udex",},
				new Artist{Name="Wilkinson",},
				new Artist{Name="Netsky",},
				new Artist{Name="KREAM",},
				new Artist{Name="Elvis Presley",},
			};

			foreach (Artist a in artists)
			{
				context.Artists.Add(a);
			}

			context.SaveChanges();

			#endregion

			#region Seed Genres

			var genres = new Genre[]
			{
				new Genre{Name="Tech House", },
				new Genre{Name="Rock & Roll",},
				new Genre{Name="Blues",},
				new Genre{Name="Funk",},
				new Genre{Name="Swing",},
			};

			foreach (Genre g in genres)
			{
				context.Genres.Add(g);
			}

			context.SaveChanges();

			#endregion

			#region Seed Songs

			var songs = new Song[]
			{
				new Song { Title="Losing It", GenreId=1,},
				new Song { Title="You Didn't", GenreId=1,},
				new Song { Title="Heartbreak Hotel", GenreId=3,},
				new Song { Title="Respect", GenreId=4,},
				new Song { Title="Cherry Cherry", GenreId=5},
				new Song { Title="All Shook Up", GenreId=3},
			};

			foreach (Song s in songs)
			{
				context.Songs.Add(s);
			}

			context.SaveChanges();

			#endregion

			#region Seed SongArtists

			var songArtists = new SongArtist[]
			{
				new SongArtist { SongId = 1, ArtistId = 1 },
				new SongArtist { SongId = 2, ArtistId = 2 },
				new SongArtist { SongId = 3, ArtistId = 3 },
				new SongArtist { SongId = 4, ArtistId = 2 },
				new SongArtist { SongId = 5, ArtistId = 5 },
				new SongArtist { SongId = 6, ArtistId = 4 },
			};

			foreach (SongArtist sa in songArtists)
			{
				context.SongArtists.Add(sa);
			}

			context.SaveChanges();

			#endregion

		}
	}
}
