using Microsoft.EntityFrameworkCore;
using Music.db.Models;

namespace Music.db.Data
{
    public class MusicdbContext : DbContext
    {
        //MusicDbContext inherrits from DbContext and forsees database functionality
        public MusicdbContext(DbContextOptions<MusicdbContext> options) : base(options) { }


        //DbSets that contain data
        //public DbSet<Artist> Artists { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Song> Songs { get; set; }
        //public DbSet<SongArtist> SongArtists { get; set; }


        //OnModelCreating methode that provides a translation to the database
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Artist>()
            //    .ToTable(nameof(Artist))
            //    .Property(a => a.Name).IsRequired();

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

            //modelBuilder.Entity<SongArtist>()
            //    .ToTable(nameof(SongArtist));

            #region Relations

            #region Song to Genre
            modelBuilder.Entity<Song>()
                .HasOne(s => s.Genre)
                .WithMany(g => g.Songs)
                .HasForeignKey(s => s.GenreId)
                .HasConstraintName("FK_GenreId");
            #endregion

            #endregion
        }
    }
}
