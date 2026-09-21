using MovieDomain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MovieDomain.Interfaces
{
    public interface IMovieRepository
    {
        Task<ICollection<Movie>> GetAllMovies();
        Task AddMovie(Movie movie);
        Task<Movie> GetMovieById(int id);
    }
}
