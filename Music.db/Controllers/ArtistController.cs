using Microsoft.AspNetCore.Mvc;
using Music.db.Models;

namespace Music.db.Controllers
{
    public class ArtistController : Controller
    {
        public List<Artist> artists;

        public ArtistController()
        {
            artists = new List<Artist>() {
                new Artist() { Id = 1, Name = "Fisher" },
                new Artist() { Id = 2, Name = "Nora En Pure" },
                new Artist() { Id = 3, Name = "JYYE" },
                new Artist() { Id = 4, Name = "Eden Prince" },
                new Artist() { Id = 5, Name = "Weiss" }
            };
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
