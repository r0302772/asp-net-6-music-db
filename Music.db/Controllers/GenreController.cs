using Microsoft.AspNetCore.Mvc;
using Music.db.Models;

namespace Music.db.Controllers
{
    public class GenreController : Controller
    {
        public GenreController()
        {

        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
