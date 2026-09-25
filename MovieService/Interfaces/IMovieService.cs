using System;
using System.Collections.Generic;
using System.Text;

namespace MovieService.Interfaces
{
    public interface IMovieService 
    {
        Task<ICollection<MovieDomain.DTOs.MovieDTO>> GetAllMovies();
        Task AddMovieAsync(MovieDomain.DTOs.CreateMovieDTO createMovieDTO);
        Task <MovieDomain.DTOs.MovieDTO> GetMovieById(int id);

        // update da delete methodebi
        Task UpdateMovieAsync(int id, MovieDomain.DTOs.UpdateMovieDTO updateMovieDTO);
        Task DeleteMovieAsync(int id);
        //დავაკება1 ძებნა სტუდიოს მიხედვით
        Task<ICollection<MovieDomain.DTOs.MovieDTO>> SearchMoviesByStudioAsync(int year, string studioName, int minimumActorCount);

        //დავალება2 ძებნა ქვეყნის მიხედვით
        Task<ICollection<MovieDomain.DTOs.MovieDTO>> SearchMoviesByCountryAsync(string countryName, int minimumYear, int maximumActorCount);

        // დავალება3 ძებნა advanced
        Task<ICollection<MovieDomain.DTOs.MovieDTO>> SearchMoviesAdvancedAsync(int fromYear, int toYear, string countryName, string titleText, int minimumActorCount);

    }
}
