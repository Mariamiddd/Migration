using MovieDomain.Entities;
using MovieDomain.Interfaces;
using MovieService.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using MovieDomain.DTOs; 



namespace MovieService.Implementations
{
    public class MovieService : IMovieService 
    {
        private readonly IMovieRepository _movieRepository;

        public MovieService(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        public async Task<ICollection<MovieDTO>> GetAllMovies()
        {
            var movies = await _movieRepository.GetAllMovies();

            var movieDtos = movies.Select(m => new MovieDTO
            {
                Title = m.Title,
                ReleaseYear = m.ReleaseYear,
                StudioName = m.Studio.Name,

            }).ToList();

            return movieDtos;
        }

        public async Task AddMovieAsync(CreateMovieDTO movieDto)
        {
            if (movieDto == null)
            {
                throw new ArgumentNullException(nameof(movieDto));
            }
            if(string.IsNullOrWhiteSpace(movieDto.Title))
            {
                throw new ArgumentException("Movie title cannot be null or empty.", nameof(movieDto.Title));
            }
            if (movieDto.ReleaseYear < 1888 || movieDto.ReleaseYear > DateTime.Now.Year)
            {
                throw new ArgumentOutOfRangeException(nameof(movieDto.ReleaseYear), "Release year must be between 1888 and the current year.");
            }
            var movie = new Movie
            {
                Title = movieDto.Title,
                ReleaseYear = movieDto.ReleaseYear,
                StudioId = movieDto.StudioId
            };
            await _movieRepository.AddMovie(movie);
        }
    }
}
