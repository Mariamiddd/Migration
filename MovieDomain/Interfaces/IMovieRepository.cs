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
        // search by studio davaleba 1
        Task<ICollection<Movie>> SearchMoviesByStudioAsync(int year, string studioName, int minimumActorCount);

        // search by country davaleba 2
        Task<ICollection<Movie>> SearchMoviesByCountryAsync(string countryName, int minimumYear, int maximumActorCount);

        //search advanced async davaleba 3
        Task<ICollection<Movie>> SearchMoviesAdvancedAsync(int fromYear, int toYear, string countryName, string titleText, int minimumActorCount);

    }
}
