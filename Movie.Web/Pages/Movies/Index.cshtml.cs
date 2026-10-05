using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieDomain.DTOs;
using MovieService.Interfaces;

namespace Movie.Web.Pages.Movies
{
    public class IndexModel : PageModel
    {
        private readonly IMovieService _movieService;

        public IndexModel(IMovieService movieService)
        {
            _movieService = movieService;
        }

        public ICollection<MovieDTO> Movies { get; set; } = new List<MovieDTO>();

        // Search Filter Properties (bound via GET query string)
        [BindProperty(SupportsGet = true)]
        public int? Year { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? StudioName { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? MinimumActorCount { get; set; }

        public async Task OnGetAsync()
        {
            // Check if user submitted any search filters
            bool hasSearchFilter = Year.HasValue
                                || !string.IsNullOrWhiteSpace(StudioName)
                                || MinimumActorCount.HasValue;

            if (hasSearchFilter)
            {
                try
                {
                    // Convert nullable types to their actual values or defaults
                    int searchYear = Year ?? 0;
                    string searchStudio = StudioName ?? string.Empty;
                    int searchMinActors = MinimumActorCount ?? 0;

                    var searchResults = await _movieService.SearchMoviesByStudioAsync(
                        searchYear, searchStudio, searchMinActors);

                    // Map search results to MovieDTO
                    Movies = searchResults.Select(x => new MovieDTO
                    {
                        Id = x.Id, // Ensure Id is mapped so Edit/Delete buttons work!
                        Title = x.Title,
                        StudioName = x.StudioName,
                        ReleaseYear = x.ReleaseYear
                    }).ToList();
                }
                catch (ArgumentException ex)
                {
                    TempData["Error"] = ex.Message;
                    Movies = new List<MovieDTO>(); // Provide empty list on error
                }
            }
            else
            {
                // Default: Load ALL movies when the search bar is untouched or "Clear Filters" is clicked
                Movies = await _movieService.GetAllMovies();
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            try
            {
                await _movieService.DeleteMovieAsync(id);
                TempData["Success"] = "Movie deleted successfully.";
            }
            catch (ArgumentException ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToPage();
        }
    }
}