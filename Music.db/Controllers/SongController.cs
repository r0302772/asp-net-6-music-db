using Microsoft.AspNetCore.Mvc;
using Music.db.Models;

namespace Music.db.Controllers
{
    public class SongController : Controller
    {
        public SongController()
        {

        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
