using MovieDomain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MovieDomain.Interfaces
{
    public interface IMovieRepository
    {
        Task<ICollection<Movie>> GetAllMoviesAsync();
        Task AddMovieAsync(Movie movie);

        Task<Movie> GetMovieByIdAsync(int id);


        Task UpdateMovieAsync(int id, Movie movie);
        Task DeleteMovieAsync(int id);
    }
}
