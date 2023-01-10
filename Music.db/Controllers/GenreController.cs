using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Music.db.Data.UnitOfWork;
using Music.db.Models;
using Music.db.ViewModels.Artist;
using Music.db.ViewModels.Genre;

namespace Music.db.Controllers
{
	public class GenreController : Controller
	{
		private readonly IUnitOfWork _uow;
		public GenreController(IUnitOfWork uow)
		{
			_uow = uow;
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
			var allGenres = _uow.GenreRepository.GetAll().OrderBy(x => x.Name).ToList();
			string[] genres = new string[allGenres.Count];

			for (int i = 0; i < allGenres.Count; i++)
			{
				genres[i] = allGenres[i].Name;

			}

			CreateGenreViewModel viewModel = new CreateGenreViewModel()
			{
				Genres = genres
			};

			return View(viewModel);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CreateGenreViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				_uow.GenreRepository.Create(new Genre()
				{
					Name = viewModel.Name,
				});

				await _uow.Save();

				return RedirectToAction(nameof(List));
			}

			return View(viewModel);
		}
		#endregion

		#region Delete

		public async Task<IActionResult> Delete(int? id)
		{
			Genre genre = await _uow.GenreRepository.GetById(id);

			if (id == null) return NotFound();

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
			Genre genre = await _uow.GenreRepository.GetById(id);

			if (id == null) return NotFound();

			if (genre == null) return NotFound();

			_uow.GenreRepository.Delete(genre);

			await _uow.Save();

			GenreListViewModel viewModel = new GenreListViewModel()
			{
				Genres = await _uow.GenreRepository.GetAll().ToListAsync()
			};
			return View(nameof(List), viewModel);
		}

		#endregion

		#region Details

		public async Task<IActionResult> Details(int? id)
		{
			Genre genre = await _uow.GenreRepository.GetById(id);

			if (id == null) return NotFound();

			if (genre != null)
			{
				IEnumerable<Song> songs = 
					await _uow.SongRepository
					.GetAll()
					.Where(x => x.GenreId == id)
					.ToListAsync();

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
					Genres = await _uow.GenreRepository.GetAll().ToListAsync()
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
				Genres = await _uow.GenreRepository.GetAll().ToListAsync()
			};

			return View(viewModel);
		}

		#endregion,

		#region Update

		public async Task<IActionResult> Update(int? id)
		{
			Genre genre = await _uow.GenreRepository.GetById(id);

			if (id == null) return NotFound();

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

					_uow.GenreRepository.Update(genre);
					await _uow.Save();

				}
				catch (DbUpdateConcurrencyException)
				{
					if (!_uow.GenreRepository.GetAll().Any(x => x.Id == viewModel.GenreId))
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

			return View(viewModel);
		}

		#endregion
	}
}
