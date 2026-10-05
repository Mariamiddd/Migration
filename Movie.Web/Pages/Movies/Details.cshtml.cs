using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieDomain.DTOs;
using MovieService.Interfaces;

namespace Movie.Web.Pages.Movies
{
    public class DetailsModel : PageModel
    {

        private readonly IMovieService _movieService;

        public DetailsModel(IMovieService movieService)
        {
            _movieService = movieService;
        }

        public MovieDTO Movie { get; set; }

        public async Task OnGetAsync(int id)
        {
            Movie = await _movieService.GetMovieById(id);

            //return Page();
        }
    }
}
