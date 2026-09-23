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
        public async Task AddMovie(Movie movie)
        {
            await _movieDbContext.Movies.AddAsync(movie);
            await _movieDbContext.SaveChangesAsync();
        }

        public async Task<ICollection<Movie>> GetAllMovies()
        {
            return await _movieDbContext.Movies
                .Include(m => m.Studio)
                .ToListAsync();
        }

        public async Task<Movie> GetMovieById(int id)
        {
            return await _movieDbContext.Movies
                .Include(m => m.Studio)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        //  update da delete
        public async Task UpdateMovie(Movie movie)
        {

            _movieDbContext.Movies.Update(movie);
            await _movieDbContext.SaveChangesAsync();
        }

        public async Task DeleteMovie(Movie movie)
        {
            _movieDbContext.Movies.Remove(movie);
            await _movieDbContext.SaveChangesAsync();
        }
    }
}
