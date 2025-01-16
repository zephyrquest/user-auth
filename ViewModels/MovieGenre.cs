using Microsoft.AspNetCore.Mvc.Rendering;
using UserAuth.Models;

namespace UserAuth.ViewModels
{
    public class MovieGenre
    {
        public List<Movie>? Movies { get; set; }
        public SelectList? Genres { get; set; }
        public string? Genre { get; set; }
        public string? Title { get; set; }
    }
}
