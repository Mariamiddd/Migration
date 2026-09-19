using System;
using System.Collections.Generic;
using System.Text;

namespace MovieService.Interfaces
{
    public interface IMovieService 
    {
        Task<ICollection<MovieDomain.DTOs.MovieDTO>> GetAllMovies();
        Task AddMovieAsync(MovieDomain.DTOs.CreateMovieDTO createMovieDTO);
    }
}
