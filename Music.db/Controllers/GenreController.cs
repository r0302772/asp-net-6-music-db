using Microsoft.AspNetCore.Mvc;
using Music.db.Models;

namespace Music.db.Controllers
{
    public class GenreController : Controller
    {
        public List<Genre> genres;

        public GenreController()
        {
            genres = new List<Genre>();
            genres.Add(new Genre() { Id = 1, Name = "Hard Dance" });
            genres.Add(new Genre() { Id = 2, Name = "House" });
            genres.Add(new Genre() { Id = 3, Name = "Funky House" });
            genres.Add(new Genre() { Id = 4, Name = "Electro House" });
            genres.Add(new Genre() { Id = 5, Name = "Tech House" });
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
