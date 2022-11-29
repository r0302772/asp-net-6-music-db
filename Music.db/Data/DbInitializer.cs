using Music.db.Models;

namespace Music.db.Data
{
	public static class DbInitializer
	{
		public static void Initialize(MusicdbContext context)
		{
			context.Database.EnsureCreated();

			if (context.Genres.Any()) return;
			if (context.Songs.Any()) return;

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
		}
	}
}
