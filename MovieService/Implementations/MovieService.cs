using MovieDomain.Entities;
using MovieDomain.Interfaces;
using MovieService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

            var movieDTO = movies.Select(m => new MovieDTO
            {
                Id = m.Id, 
                Title = m.Title,
                ReleaseYear = m.ReleaseYear,
                StudioName = m.Studio.Name,

            }).ToList();

            return movieDTO;
        }

        public async Task<MovieDTO> GetMovieById(int id)
        {
            var movie = await _movieRepository.GetMovieById(id);
            if (movie == null)
            {
                return null;
            }
            var movieDto = new MovieDTO
            {
                Id = movie.Id,
                Title = movie.Title,
                ReleaseYear = movie.ReleaseYear,
                StudioName = movie.Studio.Name
            };
            return movieDto;
        }

        public async Task AddMovieAsync(CreateMovieDTO movieDto)
        {
            if (movieDto == null)
            {
                throw new ArgumentNullException(nameof(movieDto));
            }
            if (string.IsNullOrWhiteSpace(movieDto.Title))
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

        public async Task UpdateMovieAsync(UpdateMovieDTO updateMovieDTO)
        {
            if (updateMovieDTO == null)
                throw new ArgumentNullException(nameof(updateMovieDTO));

            if (string.IsNullOrWhiteSpace(updateMovieDTO.Title))
                throw new ArgumentException("Movie title cannot be null or empty.", nameof(updateMovieDTO.Title));

            // check movie in the database
            var existingMovie = await _movieRepository.GetMovieById(updateMovieDTO.Id);
            if (existingMovie == null)
            {
                throw new KeyNotFoundException($"Movie with Id {updateMovieDTO.Id} was not found.");
            }

            // update the movie properties
            existingMovie.Title = updateMovieDTO.Title;
            existingMovie.ReleaseYear = updateMovieDTO.ReleaseYear;
            existingMovie.StudioId = updateMovieDTO.StudioId;

            await _movieRepository.UpdateMovie(existingMovie);
        }

        public async Task DeleteMovieAsync(int id)
        {
            // cehck if the movie exists
            var existingMovie = await _movieRepository.GetMovieById(id);
            if (existingMovie == null)
            {
                throw new KeyNotFoundException($"Movie with Id {id} was not found.");
            }

            // delete the movie
            await _movieRepository.DeleteMovie(existingMovie);
        }

    }
}