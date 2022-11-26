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
		public IActionResult Index()
		{
			return View();
		}

		#region Create
		public async Task<IActionResult> Create()
		{
			CreateSongViewModel viewModel = new CreateSongViewModel()
			{
				Genres = new SelectList(await _context.Genres.OrderBy(x => x.Name).ToListAsync(), "Id", "Name")
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

			return View(viewModel);
		}
		#endregion

		public IActionResult List()
		{
			var songs = _context.Songs.Include(x => x.Genre).ToList();

			return View(songs);
		}
	}
}
