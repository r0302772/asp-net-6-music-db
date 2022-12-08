using Music.db.Data.Repository;
using Music.db.Models;

namespace Music.db.Data.UnitOfWork
{
	public interface IUnitOfWork
	{
		#region Repository
		IGenericRepository<Song> SongRepository { get; }
		IGenericRepository<Genre> GenreRepository { get; }
		#endregion

		Task Save();
	}
}
