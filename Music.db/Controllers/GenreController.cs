using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Music.db.Data;
using Music.db.Models;
using Music.db.ViewModels.Genre;
using Music.db.ViewModels.Song;

namespace Music.db.Controllers
{
	public class GenreController : Controller
	{
		private readonly MusicdbContext _context;
		public GenreController(MusicdbContext context)
		{
			_context = context;
		}
		#region Index

		public IActionResult Index()
		{
			return View();
		}

		#endregion

		#region Create
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CreateGenreViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				_context.Add(new Genre()
				{
					Name = viewModel.Name,
				});

				await _context.SaveChangesAsync();

				return RedirectToAction(nameof(List));
			}

			return View(viewModel);
		}
		#endregion

		#region Delete

		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null) return NotFound();

			var genre = await _context.Genres.FirstOrDefaultAsync(x => x.Id == id);

			if (genre == null) return NotFound();

			DeleteGenreViewModel viewModel = new DeleteGenreViewModel()
			{
				GenreId = genre.Id,
				Name = genre.Name,
			};

			return View(nameof(Delete), viewModel);
		}


		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int? id)
		{
			if (id == null) return NotFound();

			var genre = await _context.Genres.FindAsync(id);

			if (genre == null) return NotFound();

			_context.Genres.Remove(genre);

			await _context.SaveChangesAsync();

			GenreListViewModel viewModel = new GenreListViewModel()
			{
				Genres = await _context.Genres.ToListAsync()
			};
			return View(nameof(List), viewModel);
		}

		#endregion

		#region Details

		public async Task<IActionResult> Details(int? id)
		{
			if (id == null) return NotFound();

			var genre = await _context.Genres.Where(x => x.Id == id).FirstOrDefaultAsync();
			var songs = await _context.Songs.Where(x => x.GenreId == id).ToListAsync();

			if (genre != null)
			{
				GenreDetailsViewModel viewModel = new GenreDetailsViewModel()
				{
					Name = genre.Name,
					Songs = songs,
				};
				return View(nameof(Details), viewModel);
			}
			else
			{
				GenreListViewModel viewModel = new GenreListViewModel()
				{
					Genres = _context.Genres.ToList()
				};
				return View(nameof(List), viewModel);
			}
		}

		#endregion

		#region List

		public async Task<IActionResult> List()
		{
			GenreListViewModel viewModel = new GenreListViewModel()
			{
				Genres = await _context.Genres.ToListAsync()
			};

			return View(viewModel);
		}

		#endregion,

		#region Update

		public async Task<IActionResult> Update(int? id)
		{
			if (id == null) return NotFound();

			var genre = await _context.Genres.FindAsync(id);

			if (genre == null) return NotFound();

			UpdateGenreViewModel viewModel = new UpdateGenreViewModel()
			{
				GenreId = genre.Id,
				Name = genre.Name,
			};

			return View(viewModel);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Update(int id, UpdateGenreViewModel viewModel)
		{
			if (id != viewModel.GenreId) return NotFound();

			if (ModelState.IsValid)
			{
				try
				{
					Genre genre = new Genre()
					{
						Id = viewModel.GenreId,
						Name = viewModel.Name,
					};

					_context.Update(genre);
					await _context.SaveChangesAsync();

				}
				catch (DbUpdateConcurrencyException)
				{
					if (!_context.Genres.Any(x => x.Id == viewModel.GenreId))
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

			return View(viewModel);
		}

		#endregion
	}
}
