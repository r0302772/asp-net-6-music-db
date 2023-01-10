using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Music.db.Data;
using Music.db.Data.UnitOfWork;
using Music.db.Models;
using Music.db.ViewModels.Song;

namespace Music.db.Controllers
{
	public class SongController : Controller
	{
		private readonly IUnitOfWork _uow;

		public SongController(IUnitOfWork uow)
		{
			_uow = uow;
		}

		#region Index

		public async Task<IActionResult> Index()
		{
			SongViewModel viewModel = new SongViewModel()
			{
				Genres = new SelectList(await _uow.GenreRepository.GetAll().OrderBy(x => x.Name).ToListAsync(), "Id", "Name")
			};

			return View(viewModel);
		}

		#endregion

		#region Create
		public async Task<IActionResult> Create()
		{
			var artists = await _uow.ArtistRepository.GetAll().OrderBy(x => x.Name).ToListAsync();
			var remixers = await _uow.ArtistRepository.GetAll().OrderBy(x => x.Name).ToListAsync();

			remixers.Add(new Artist { Name = "Original version." });
			var genres = await _uow.GenreRepository.GetAll().OrderBy(x => x.Name).ToListAsync();


			SongViewModel viewModel = new SongViewModel()
			{
				Genres = new SelectList(genres, "Id", "Name"),
				Artists = new MultiSelectList(artists, "Id", "Name"),
				Remixers = new SelectList(remixers.OrderBy(x => x.Name), "Id", "Name", 0),
			};

			return View(viewModel);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(SongViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				Song song = new Song()
				{
					Title = viewModel.Title,
					GenreId = viewModel.GenreId,
					RemixerId = (viewModel.RemixerId == 0) ? null : viewModel.RemixerId,
				};

				_uow.SongRepository.Create(song);
				await _uow.Save();

				foreach (var artistId in viewModel.ArtistsIds)
				{
					SongArtist sa = new SongArtist()
					{
						SongId = song.Id,
						ArtistId = artistId
					};

					_uow.SongArtistRepository.Create(sa);
					await _uow.Save();
				}

				return RedirectToAction(nameof(List));
			}

			return View(nameof(Index));
		}
		#endregion

		#region Delete
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null) return NotFound();

			var song = await _uow.SongRepository.GetById(id);

			if (song == null) return NotFound();

			DeleteSongViewModel viewModel = new DeleteSongViewModel()
			{
				SongId = song.Id,
				Title = song.Title,
			};

			return View(nameof(Delete), viewModel);
		}


		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int? id)
		{
			if (id == null) return NotFound();

			var songArtists = await _uow.SongArtistRepository.GetAll().Where(x => x.SongId == id).ToListAsync();

			foreach (var songArtist in songArtists)
			{
				_uow.SongArtistRepository.Delete(songArtist);
			}
			var song = await _uow.SongRepository.GetById(id);

			if (song == null) return NotFound();

			_uow.SongRepository.Delete(song);

			await _uow.Save();

			SongListViewModel viewModel = new SongListViewModel()
			{
				Songs = await _uow.SongRepository.GetAll()
												 .Include(x => x.Genre)
												 .ToListAsync(),
				SongArtists = await _uow.SongArtistRepository.GetAll()
															 .Include(x => x.Song)
															 .Include(x => x.Artist)
															 .ToListAsync()
			};
			return View(nameof(List), viewModel);
		}

		#endregion

		#region Details

		public async Task<IActionResult> Details(int? id)
		{
			if (id == null) return NotFound();

			var song = await _uow.SongRepository.GetAll().Include(x => x.Genre).Where(x => x.Id == id).FirstOrDefaultAsync();

			if (song != null)
			{
				SongDetailsViewModel viewModel = new SongDetailsViewModel()
				{
					Title = song.Title,
					GenreId = song.GenreId,
				};
				return View(nameof(Details), viewModel);
			}
			else
			{
				SongListViewModel viewModel = new SongListViewModel()
				{
					Songs = await _uow.SongRepository.GetAll().Include(x => x.Genre).ToListAsync()
				};
				return View(nameof(List), viewModel);
			}
		}

		#endregion

		#region List

		public async Task<IActionResult> List()
		{
			SongListViewModel viewModel = new SongListViewModel()
			{
				Songs = await _uow.SongRepository.GetAll()
												 .Include(x => x.Genre)
												 .Include(x => x.Remixer)
												 .ToListAsync(),
				SongArtists = await _uow.SongArtistRepository.GetAll()
															 .Include(x => x.Song)
															 .Include(x => x.Artist)
															 .ToListAsync()
			};

			return View(viewModel);
		}

		#endregion

		#region Update

		public async Task<IActionResult> Update(int? id)
		{
			if (id == null) return NotFound();

			var song = await _uow.SongRepository.GetById(id);

			if (song == null) return NotFound();

			var songArtists = await _uow.SongArtistRepository.GetAll().Where(x => x.SongId == song.Id).ToListAsync();

			var artists = await _uow.ArtistRepository.GetAll().OrderBy(x => x.Name).ToListAsync();
			var remixers = await _uow.ArtistRepository.GetAll().OrderBy(x => x.Name).ToListAsync();

			remixers.Add(new Artist { Name = "Original version." });
			var genres = await _uow.GenreRepository.GetAll().OrderBy(x => x.Name).ToListAsync();

			SongViewModel viewModel = new SongViewModel()
			{
				SongId = song.Id,
				Title = song.Title,
				GenreId = song.GenreId,

				Genres = new SelectList(genres, "Id", "Name", song.GenreId),
				Artists = new MultiSelectList(artists, "Id", "Name", songArtists.Select(x => x.ArtistId).ToArray()),
				Remixers = new SelectList(remixers.OrderBy(x => x.Name), "Id", "Name", song.RemixerId == null ? 0 : song.RemixerId)
			};

			return View(viewModel);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Update(int id, SongViewModel viewModel)
		{
			if (id != viewModel.SongId) return NotFound();

			if (ModelState.IsValid)
			{
				try
				{
					Song song = new Song()
					{
						Id = viewModel.SongId,
						Title = viewModel.Title,
						GenreId = viewModel.GenreId,
						RemixerId = (viewModel.RemixerId == 0) ? null : viewModel.RemixerId,
					};

					_uow.SongRepository.Update(song);
					await _uow.Save();

					var songArtists = _uow.SongArtistRepository.GetAll().Where(x => x.SongId == song.Id);

					foreach (var songArtist in songArtists)
					{
						if (!viewModel.ArtistsIds.Contains(songArtist.ArtistId))
						{
							_uow.SongArtistRepository.Delete(songArtist);
						}
					}

					foreach (var artistId in viewModel.ArtistsIds)
					{
						var songArtist = songArtists.FirstOrDefault(x => x.ArtistId == artistId);

						if (songArtist == null)
						{
							SongArtist sa = new SongArtist()
							{
								SongId = song.Id,
								ArtistId = artistId
							};

							_uow.SongArtistRepository.Create(sa);
						}

						await _uow.Save();
					}
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!_uow.SongRepository.GetAll().Any(x => x.Id == viewModel.SongId))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(List));
			}

			viewModel.Genres = new SelectList(await _uow.GenreRepository.GetAll().OrderBy(x => x.Name).ToListAsync(), "Id", "Name", viewModel.GenreId);

			return View(viewModel);
		}

		#endregion
	}
}
