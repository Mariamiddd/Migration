using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieDomain.Interfaces;
using System.Threading.Tasks;
using MovieEntity = MovieDomain.Entities.Movie;

namespace Movie.Web.Pages.Movies
{
    public class EditModel : PageModel
    {
        private readonly IMovieRepository _movieRepository;

        public EditModel(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        [BindProperty]
        public MovieEntity Movie { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var movie = await _movieRepository.GetMovieByIdAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            Movie = movie;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            // Remove validation errors for properties not present in the HTML form
            ModelState.Remove("Movie.Studio");
            ModelState.Remove("Movie.Actors");

            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Ensure Id is set from the route
            Movie.Id = id;

            await _movieRepository.UpdateMovieAsync(id, Movie);

            TempData["Success"] = "Movie updated successfully.";
            return RedirectToPage("./Index");
        }
    }
}