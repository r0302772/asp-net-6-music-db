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
	}
}
