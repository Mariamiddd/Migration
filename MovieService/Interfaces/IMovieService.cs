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
    }
}
