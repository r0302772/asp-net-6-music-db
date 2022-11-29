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

        public async Task<IActionResult> List()
        {
            var songs = await _context.Songs.Include(x => x.Genre).ToListAsync();

            return View(songs);
        }
    }
}
