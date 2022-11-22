namespace Music.db.ViewModels.Genre
{
    using Music.db.Models;   
    public class GenreListViewModel
    {
        public string GenreSearch { get; set; }
        public List<Genre> Genres { get; set; }
    }
}
