using MovieDomain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MovieDomain.Interfaces
{
    public interface IMovieRepository
    {
        Task<ICollection<Movie.Domain.Entities.Movie>> GetAllMoviesAsync();
        Task AddMovieAsync(Movie.Domain.Entities.Movie movie);

        Task<Movie.Domain.Entities.Movie> GetMovieByIdAsync(int id);


        Task UpdateMovieAsync(int id, Domain.Entities.Movie movie);
        Task DeleteMovieAsync(int id);
    }
}
