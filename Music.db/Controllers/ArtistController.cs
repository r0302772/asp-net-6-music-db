using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Music.db.Data.UnitOfWork;
using Music.db.Models;
using Music.db.ViewModels.Artist;
using Music.db.ViewModels.Genre;

namespace Music.db.Controllers
{
    public class ArtistController : Controller
    {
		private readonly IUnitOfWork _uow;
		public ArtistController(IUnitOfWork uow)
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
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CreateArtistViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				_uow.ArtistRepository.Create(new Artist()
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
			Artist artist = await _uow.ArtistRepository.GetById(id);

			if (id == null) return NotFound();

			if (artist == null) return NotFound();

			DeleteArtistViewModel viewModel = new DeleteArtistViewModel()
			{
				ArtistId = artist.Id,
				Name = artist.Name,
			};

			return View(nameof(Delete), viewModel);
		}


		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int? id)
		{
			Artist artist = await _uow.ArtistRepository.GetById(id);

			if (id == null) return NotFound();

			if (artist == null) return NotFound();

			_uow.ArtistRepository.Delete(artist);

			await _uow.Save();

			ArtistListViewModel viewModel = new ArtistListViewModel()
			{
				Artists = await _uow.ArtistRepository.GetAll().ToListAsync()
			};
			return View(nameof(List), viewModel);
		}

		#endregion

		#region Details

		public async Task<IActionResult> Details(int? id)
		{
			Artist artist = await _uow.ArtistRepository.GetById(id);

			if (id == null) return NotFound();

			if (artist != null)
			{
				//IEnumerable<Song> songs =
				//	await _uow.SongRepository
				//	.GetAll()
				//	.Where(x => x.GenreId == id)
				//	.ToListAsync();

				ArtistDetailsViewModel viewModel = new ArtistDetailsViewModel()
				{
					Name = artist.Name,
					//Songs = songs,
				};

				return View(nameof(Details), viewModel);
			}
			else
			{
				ArtistListViewModel viewModel = new ArtistListViewModel()
				{
					Artists = await _uow.ArtistRepository.GetAll().ToListAsync()
				};
				return View(nameof(List), viewModel);
			}
		}

		#endregion

		#region List

		public async Task<IActionResult> List()
		{
			ArtistListViewModel viewModel = new ArtistListViewModel()
			{
				Artists = await _uow.ArtistRepository.GetAll().ToListAsync()
			};

			return View(viewModel);
		}

		#endregion,

		#region Update

		public async Task<IActionResult> Update(int? id)
		{
			Artist artist = await _uow.ArtistRepository.GetById(id);

			if (id == null) return NotFound();

			if (artist == null) return NotFound();

			UpdateArtistViewModel viewModel = new UpdateArtistViewModel()
			{
				ArtistId = artist.Id,
				Name = artist.Name,
			};

			return View(viewModel);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Update(int id, UpdateArtistViewModel viewModel)
		{
			if (id != viewModel.ArtistId) return NotFound();

			if (ModelState.IsValid)
			{
				try
				{
					Artist artist = new Artist()
					{
						Id = viewModel.ArtistId,
						Name = viewModel.Name,
					};

					_uow.ArtistRepository.Update(artist);
					await _uow.Save();

				}
				catch (DbUpdateConcurrencyException)
				{
					if (!_uow.ArtistRepository.GetAll().Any(x => x.Id == viewModel.ArtistId))
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
