using Microsoft.AspNetCore.Mvc;

namespace Music.db.Controllers
{
    public class ArtistController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
