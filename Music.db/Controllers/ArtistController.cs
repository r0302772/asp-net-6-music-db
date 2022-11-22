using Microsoft.AspNetCore.Mvc;
using Music.db.Models;

namespace Music.db.Controllers
{
    public class ArtistController : Controller
    {
        public List<Artist> artists;

        public ArtistController()
        {
            artists = new List<Artist>();
            artists.Add(new Artist() { Id = 1, Name = "Fisher" });
            artists.Add(new Artist() { Id = 2, Name = "Nora En Pure" });
            artists.Add(new Artist() { Id = 3, Name = "JYYE" });
            artists.Add(new Artist() { Id = 4, Name = "Eden Prince" });
            artists.Add(new Artist() { Id = 5, Name = "Weiss" });
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
