using Microsoft.AspNetCore.Mvc;

namespace Music.db.Controllers
{
    public class GenreController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
