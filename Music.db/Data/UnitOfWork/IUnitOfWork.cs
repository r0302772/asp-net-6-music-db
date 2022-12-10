using Music.db.Data.Repository;
using Music.db.Models;

namespace Music.db.Data.UnitOfWork
{
	public interface IUnitOfWork
	{
		#region Repository
		IGenericRepository<Artist> ArtistRepository { get; }
		IGenericRepository<Genre> GenreRepository { get; }
		IGenericRepository<Song> SongRepository { get; }
		IGenericRepository<SongArtist> SongArtistRepository { get; }
		#endregion

		Task Save();
	}
}
