using Microsoft.AspNetCore.Mvc;

namespace Music.db.Controllers
{
    public class SongController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
