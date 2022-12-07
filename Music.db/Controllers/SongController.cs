using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Music.db.Data;
using Music.db.Models;
using Music.db.ViewModels.Song;

namespace Music.db.Controllers
{
	public class SongController : Controller
	{
		private readonly MusicdbContext _context;
		public SongController(MusicdbContext context)
		{
			_context = context;
		}

		#region Index

		public async Task<IActionResult> Index()
		{
			CreateSongViewModel viewModel = new CreateSongViewModel()
			{
				Genres = new SelectList(await _context.Genres.OrderBy(x => x.Name).ToListAsync(), "Id", "Name")
			};

			return View(viewModel);
		}

		#endregion

		#region Create
		public IActionResult Create()
		{
			CreateSongViewModel viewModel = new CreateSongViewModel()
			{
				Genres = new SelectList(_context.Genres.OrderBy(x => x.Name).ToList(), "Id", "Name")
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

				_context.Add(song);
				await _context.SaveChangesAsync();

				return RedirectToAction(nameof(List));
			}

			viewModel.Genres = new SelectList(await _context.Genres.OrderBy(x => x.Name).ToListAsync(), "Id", "Name");

			return View(nameof(Index));
		}
		#endregion

		#region Delete
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null) return NotFound();

			var song = await _context.Songs.FirstOrDefaultAsync(x => x.Id == id);

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

			var song = await _context.Songs.FindAsync(id);

			if (song == null) return NotFound();

			_context.Songs.Remove(song);

			await _context.SaveChangesAsync();

			SongListViewModel viewModel = new SongListViewModel()
			{
				Songs = await _context.Songs.Include(x => x.Genre).ToListAsync()
			};
			return View(nameof(List), viewModel);
		}

		#endregion

		#region Details

		public async Task<IActionResult> Details(int? id)
		{
			if (id == null) return NotFound();

			var song = await _context.Songs.Include(x => x.Genre).Where(x => x.Id == id).FirstOrDefaultAsync();

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
					Songs = _context.Songs.ToList()
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
				Songs = await _context.Songs.Include(x => x.Genre).ToListAsync()
			};

			return View(viewModel);
		}

		#endregion

		#region Update

		public async Task<IActionResult> Update(int? id)
		{
			if (id == null) return NotFound();

			var song = await _context.Songs.FindAsync(id);

			if (song == null) return NotFound();

			UpdateSongViewModel viewModel = new UpdateSongViewModel()
			{
				SongId = song.Id,
				Title = song.Title,
				GenreId= song.GenreId,

				Genres = new SelectList(await _context.Genres.OrderBy(x => x.Name).ToListAsync(), "Id", "Name", song.GenreId),
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

					_context.Update(song);
					await _context.SaveChangesAsync();

				}
				catch (DbUpdateConcurrencyException)
				{
					if (!_context.Songs.Any(x => x.Id == viewModel.SongId))
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

			viewModel.Genres = new SelectList(await _context.Genres.OrderBy(x => x.Name).ToListAsync(), "Id", "Name", viewModel.GenreId);

			return View(viewModel);
		}

		#endregion
	}
}
