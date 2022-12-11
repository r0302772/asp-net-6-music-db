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
			CreateSongViewModel viewModel = new CreateSongViewModel()
			{
				Genres = new SelectList(await _uow.GenreRepository.GetAll().OrderBy(x => x.Name).ToListAsync(), "Id", "Name")
			};

			return View(viewModel);
		}

		#endregion

		#region Create
		public IActionResult Create()
		{
			CreateSongViewModel viewModel = new CreateSongViewModel()
			{
				Genres = new SelectList(_uow.GenreRepository.GetAll().OrderBy(x => x.Name).ToList(), "Id", "Name"),
				Artists = new MultiSelectList(_uow.ArtistRepository.GetAll().OrderBy(x => x.Name), "Id", "Name")
			};

			return View(viewModel);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CreateSongViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				Song song = new Song()
				{
					Title = viewModel.Title,
					GenreId = viewModel.GenreId,
				};

				_uow.SongRepository.Create(song);
				await _uow.Save();

				foreach (var artist in viewModel.ArtistId)
				{
					SongArtist sa = new SongArtist()
					{
						SongId = song.Id,
						ArtistId = artist
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

			var song = await _uow.SongRepository.GetById(id);

			if (song == null) return NotFound();

			_uow.SongRepository.Delete(song);

			await _uow.Save();

			SongListViewModel viewModel = new SongListViewModel()
			{
				Songs = await _uow.SongRepository.GetAll().Include(x => x.Genre).ToListAsync()
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

			UpdateSongViewModel viewModel = new UpdateSongViewModel()
			{
				SongId = song.Id,
				Title = song.Title,
				GenreId = song.GenreId,

				Genres = new SelectList(await _uow.GenreRepository.GetAll().OrderBy(x => x.Name).ToListAsync(), "Id", "Name", song.GenreId),
				Artists = new MultiSelectList(await _uow.ArtistRepository.GetAll().OrderBy(x => x.Name).ToListAsync(), "Id", "Name", songArtists.Select(x => x.ArtistId).ToArray())
			};

			return View(viewModel);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Update(int id, UpdateSongViewModel viewModel)
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
					};

					_uow.SongRepository.Update(song);
					await _uow.Save();

					var songArtists = await _uow.SongArtistRepository.GetAll().Where(x => x.SongId == song.Id).ToListAsync();

					foreach (var artist in viewModel.ArtistId)
					{
						SongArtist sa = new SongArtist()
						{
							SongId = song.Id,
							ArtistId = artist
						};

						_uow.SongArtistRepository.Update(sa);
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
				return RedirectToAction(nameof(Index));
			}

			viewModel.Genres = new SelectList(await _uow.GenreRepository.GetAll().OrderBy(x => x.Name).ToListAsync(), "Id", "Name", viewModel.GenreId);

			return View(viewModel);
		}

		#endregion
	}
}
