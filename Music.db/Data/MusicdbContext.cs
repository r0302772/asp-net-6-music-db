using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Music.db.Models;

namespace Music.db.Data
{
	public class MusicdbContext : IdentityDbContext<IdentityUser>
	{
		//MusicDbContext inherrits from DbContext and forsees database functionality
		public MusicdbContext(DbContextOptions<MusicdbContext> options) : base(options) { }


		//DbSets that contain data
		public DbSet<Artist> Artists { get; set; }
		public DbSet<Genre> Genres { get; set; }
		public DbSet<Song> Songs { get; set; }
		public DbSet<SongArtist> SongArtists { get; set; }


		//OnModelCreating methode that provides a translation to the database
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			#region Artist

			modelBuilder.Entity<Artist>().ToTable(nameof(Artist));
			modelBuilder.Entity<Artist>().HasKey(a => a.Id);
			modelBuilder.Entity<Artist>().Property(a => a.Name).IsRequired();

			#endregion

			#region Genre

			modelBuilder.Entity<Genre>().ToTable(nameof(Genre));
			modelBuilder.Entity<Genre>().HasKey(g => g.Id);
			modelBuilder.Entity<Genre>().Property(g => g.Name).IsRequired();

			#endregion

			#region Song

			modelBuilder.Entity<Song>().ToTable(nameof(Song));
			modelBuilder.Entity<Song>().HasKey(s => s.Id);
			modelBuilder.Entity<Song>().Property(s => s.Title).IsRequired();

			#endregion

			#region Relations

			#region Song to Genre
			modelBuilder.Entity<Song>()
				.HasOne(s => s.Genre)
				.WithMany(g => g.Songs)
				.HasForeignKey(s => s.GenreId);
			#endregion

			#region Song to Artist
			modelBuilder.Entity<SongArtist>().ToTable(nameof(SongArtist));
			modelBuilder.Entity<SongArtist>().HasKey(sa => sa.Id);

			modelBuilder.Entity<SongArtist>()
				.HasOne<Song>(sa => sa.Song)
				.WithMany(s => s.SongArtists)
				.HasForeignKey(sa => sa.SongId);


			modelBuilder.Entity<SongArtist>()
				.HasOne<Artist>(sa => sa.Artist)
				.WithMany(s => s.SongArtists)
				.HasForeignKey(sa => sa.ArtistId);
			#endregion

			#endregion
		}
	}
}
