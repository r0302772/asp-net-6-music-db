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
		public async Task<IActionResult> Index()
		{
			CreateSongViewModel viewModel = new CreateSongViewModel()
			{
				Genres = new SelectList(await _context.Genres.OrderBy(x => x.Name).ToListAsync(), "Id", "Name")
			};

			return View(viewModel);
		}

		#region Create
		//public IActionResult Create()
		//{
		//    return PartialView();
		//}

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

		#region Details

		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var song = _context.Songs.Include(x => x.Genre).Where(x => x.Id == id).FirstOrDefault();

			if (song != null)
			{
				SongListViewModel viewModel = new SongListViewModel()
				{
					Title = song.Title,
					GenreName = song.Genre.Name,
					Songs = await _context.Songs.Include(x => x.Genre).ToListAsync()
				};
				return View(nameof(List), viewModel);
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

		#region Delete
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var song = await _context.Songs.FirstOrDefaultAsync(x => x.Id == id);
			if (song == null)
			{
				return NotFound();
			}

			var songs = await _context.Songs.Include(x => x.Genre).ToListAsync();

			SongListViewModel viewModel = new SongListViewModel()
			{
				SongId = song.Id,
				Title = song.Title,
				Songs = songs
			};

			ViewBag["block"] = "block";

			return View(nameof(List),viewModel);
		}


		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var song = await _context.Songs.FindAsync(id);

			if (song == null)
			{
				return NotFound();
			}

			_context.Songs.Remove(song);

			await _context.SaveChangesAsync();

			SongListViewModel viewModel = new SongListViewModel()
			{
				Songs = await _context.Songs.Include(x => x.Genre).ToListAsync()
			};
			return View(nameof(List), viewModel);
		}

		#endregion

		public async Task<IActionResult> List()
		{
			SongListViewModel viewModel = new SongListViewModel()
			{
				Songs = await _context.Songs.Include(x => x.Genre).ToListAsync()
			};

			return View(viewModel);
		}
	}
}
