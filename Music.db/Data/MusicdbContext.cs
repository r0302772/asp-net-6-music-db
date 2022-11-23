using Microsoft.EntityFrameworkCore;
using Music.db.Models;

namespace Music.db.Data
{
    public class MusicdbContext : DbContext
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
            modelBuilder.Entity<Artist>()
                .ToTable(nameof(Artist))
                .Property(a => a.Name).IsRequired();

            modelBuilder.Entity<Genre>()
                .ToTable(nameof(Genre))
                .Property(g => g.Name).IsRequired();

            modelBuilder.Entity<Song>()
                .ToTable(nameof(Song))
                .Property(s => s.Title).IsRequired();


            modelBuilder.Entity<SongArtist>().ToTable(nameof(SongArtist));
        }
    }
}
