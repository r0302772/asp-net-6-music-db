using Music.db.Data.Repository;
using Music.db.Models;

namespace Music.db.Data.UnitOfWork
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly MusicdbContext _context;

		public UnitOfWork(MusicdbContext context)
		{
			_context = context;
		}

		#region Arist
		private IGenericRepository<Artist> artistRepository;
		public IGenericRepository<Artist> ArtistRepository
		{
			get
			{
				if (artistRepository == null)
				{
					artistRepository = new GenericRepository<Artist>(_context);
				}
				return artistRepository;
			}
		}
		#endregion

		#region Genre
		private IGenericRepository<Genre> genreRepository;
		public IGenericRepository<Genre> GenreRepository
		{
			get
			{
				if (genreRepository == null)
				{
					genreRepository = new GenericRepository<Genre>(_context);
				}
				return genreRepository;
			}
		}
		#endregion

		#region Song
		private IGenericRepository<Song> songRepository;
		public IGenericRepository<Song> SongRepository
		{
			get
			{
				if (songRepository == null)
				{
					songRepository = new GenericRepository<Song>(_context);
				}
				return songRepository;
			}
		}
		#endregion

		#region SongArtist
		private IGenericRepository<SongArtist> songArtistRepository;
		public IGenericRepository<SongArtist> SongArtistRepository
		{
			get
			{
				if (songArtistRepository == null)
				{
					songArtistRepository = new GenericRepository<SongArtist>(_context);
				}
				return songArtistRepository;
			}
		}
		#endregion

		public async Task Save() { await _context.SaveChangesAsync(); }
	}
}
