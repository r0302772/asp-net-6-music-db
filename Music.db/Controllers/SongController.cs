using Microsoft.AspNetCore.Mvc;
using Music.db.Models;

namespace Music.db.Controllers
{
    public class SongController : Controller
    {

        public List<Song> songs;

        public SongController()
        {
            songs = new List<Song>();
            songs.Add(new Song() { Id = 1, Title = "Losing It" });
            songs.Add(new Song() { Id = 2, Title = "Look At You" });
            songs.Add(new Song() { Id = 3, Title = "Señorita" });
            songs.Add(new Song() { Id = 4, Title = "Makes Me Love You" });
            songs.Add(new Song() { Id = 5, Title = "Feel My Needs" });
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
