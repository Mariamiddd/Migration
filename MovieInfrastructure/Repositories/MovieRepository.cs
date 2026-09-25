using Microsoft.EntityFrameworkCore;
using MovieDomain.Entities;
using MovieDomain.Interfaces;
using MovieInfrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;

namespace MovieInfrastructure.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly MovieDbContext _movieDbContext;
        public MovieRepository(MovieDbContext movieDbContext)
        {
            _movieDbContext = movieDbContext;
        }
        public async Task AddMovieAsync(Movie movie)
        {
            await _movieDbContext.Movies.AddAsync(movie);
        }

        public async Task<ICollection<Movie>> GetAllMoviesAsync()
        {
            return await _movieDbContext.Movies
                .Include(m => m.Studio)
                .ToListAsync();
        }

        public async Task<Movie> GetMovieByIdAsync(int id)
        {
            return await _movieDbContext.Movies
                .Include(m => m.Studio)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        //  update da delete
        public async Task UpdateMovieAsync(int id, Movie movie)
        {
            var movieExists =
                await _movieDbContext.Movies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movieExists == null)
            {
                throw new ArgumentException("Movie not found");
            }

            movieExists.Title = movie.Title;
            movieExists.ReleaseYear = movie.ReleaseYear;
            movieExists.StudioId = movie.StudioId;


        }


        public async Task DeleteMovieAsync(int id)
        {

            var movieExists = await _movieDbContext.Movies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movieExists == null)
            {
                throw new ArgumentException("Movie not found");
            }

            _movieDbContext.Movies.Remove(movieExists);

        }


        // seatch movie

        //davaleba1 
        public async Task<ICollection<Movie>> SearchMoviesByStudioAsync(int year, string studioName, int minimumActorCount)
        {
            var movies = await _movieDbContext.Movies
                .Include(m => m.Studio)
                .Include(m => m.Actors)
                .Where(m => m.ReleaseYear >= year
                         && m.Studio.Name == studioName
                         && m.Actors.Count >= minimumActorCount)
                .OrderByDescending(m => m.ReleaseYear)
                .ThenBy(m => m.Title)
                .ToListAsync();

            return movies;
        }

        //დავალება 2 ქვეყნის მიხედვით ფილმების ძებნა
        public async Task<ICollection<Movie>> SearchMoviesByCountryAsync(string CountryName, int minimumYear, int maximumActorCount)
        {
            var movies = await _movieDbContext.Movies
                .Include(m => m.Studio)
                    .ThenInclude(s => s.Country)
                .Include(m => m.Actors)
                .Where(m => m.Studio.Country.countryName == CountryName
                         && m.ReleaseYear >= minimumYear
                         && m.Actors.Count <= maximumActorCount)
                .OrderBy(m => m.Actors.Count)
                .ThenByDescending(m => m.ReleaseYear)
                .ThenBy(m => m.Title)
                .ToListAsync();

            return movies;
        }

        public async Task<ICollection<Movie>> SearchMoviesAdvancedAsync(int fromYear, int toYear, string countryName, string titleText, int minimumActorCount)
        {
            var movies = await _movieDbContext.Movies
         .Include(m => m.Studio)
             .ThenInclude(s => s.Country)
         .Include(m => m.Actors)
         .Where(m => m.ReleaseYear >= fromYear
                  && m.ReleaseYear <= toYear
                  && m.Studio.Country.countryName == countryName
                  && m.Title.Contains(titleText)
                  && m.Actors.Count >= minimumActorCount)
         .OrderByDescending(m => m.Actors.Count)
         .ThenByDescending(m => m.ReleaseYear)
         .ThenBy(m => m.Studio.Name)
         .ThenBy(m => m.Title)
         .ToListAsync();

            return movies;

        }
    }
}
