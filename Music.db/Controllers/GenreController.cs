using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Music.db.Data;
using Music.db.Models;
using Music.db.ViewModels.Genre;

namespace Music.db.Controllers
{
	public class GenreController : Controller
	{
		private readonly MusicdbContext _context;
		public GenreController(MusicdbContext context)
		{
			_context = context;
		}

		public IActionResult Index()
		{
			return View();
		}

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

		public IActionResult List()
		{
			var genres = _context.Genres.ToList();
			return View(genres);
		}
	}
}
