using Microsoft.AspNetCore.Mvc.Rendering;
using user_auth.Models;

namespace user_auth.ViewModels
{
    public class MovieGenre
    {
        public List<Movie>? Movies { get; set; }
        public SelectList? Genres { get; set; }
        public string? Genre { get; set; }
        public string? Title { get; set; }
    }
}
