using Microsoft.AspNetCore.Mvc;
using Music.db.Models;

namespace Music.db.Controllers
{
    public class ArtistController : Controller
    {
        public ArtistController()
        {

        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
