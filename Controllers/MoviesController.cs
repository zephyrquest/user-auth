using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UserAuth.Data;
using UserAuth.Models;
using UserAuth.ViewModels;

namespace UserAuth.Controllers
{
    public class MoviesController : Controller
    {
        private readonly UserAuthContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public MoviesController(UserAuthContext context, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: Movies
        public async Task<IActionResult> Index(string title, string genre)
        {
            if(_context.Movie == null)
            {
                return Problem("Entity set 'UserAuthContext.Movie'  is null.");
            }

            // define a linq query. Only defined, not run against database
            var movies = from m in _context.Movie 
                         select m;

            // Use LINQ to get list of genres.
            IQueryable<string> genreQuery = from m in _context.Movie
                                            orderby m.Genre
                                            select m.Genre;

            if (!string.IsNullOrEmpty(title))
            {
                movies = movies.Where(m => m.Title!.ToLower().Contains(title.ToLower()));
            }

            if (!string.IsNullOrEmpty(genre))
            {
                movies = movies.Where(m => m.Genre == genre);
            }

            // view model
            var movieGenre = new MovieGenre
            {
                Movies = await movies.ToListAsync(),
                Genres = new SelectList(await genreQuery.ToListAsync())
            };

            return View(movieGenre);
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // GET: Movies/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,Title,ReleaseDate,Genre,Price,Rating")] Movie movie)
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                movie.UserId = _userManager.GetUserId(User);
                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movies/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            if (movie.UserId != userId)
            {
                return Forbid();
            }

            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,ReleaseDate,Genre,Price,Rating")] Movie movie)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }

            var existingMovie = await _context.Movie.FindAsync(id);
            if (existingMovie == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            if (existingMovie.UserId != userId)
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    existingMovie.Title = movie.Title;
                    existingMovie.ReleaseDate = movie.ReleaseDate;
                    existingMovie.Genre = movie.Genre;
                    existingMovie.Price = movie.Price;
                    existingMovie.Rating = movie.Rating;

                    _context.Update(existingMovie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movies/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            if (movie.UserId != userId)
            {
                return Forbid();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movie.FindAsync(id);
            if (movie != null)
            {
                var userId = _userManager.GetUserId(User);
                if (movie.UserId != userId)
                {
                    return Forbid();
                }

                _context.Movie.Remove(movie);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
            return _context.Movie.Any(e => e.Id == id);
        }
    }
}