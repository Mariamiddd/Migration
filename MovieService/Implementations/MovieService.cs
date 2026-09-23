using MovieDomain.Entities;
using MovieDomain.Interfaces;
using MovieService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieDomain.DTOs;
using MovieService.Implementations;

namespace MovieService.Implementations
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IUnitOfWork _unitOfWork;

        public MovieService(IMovieRepository movieRepository, IUnitOfWork unitOfWork)
        {
            _movieRepository = movieRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ICollection<MovieDTO>> GetAllMovies()
        {
            var movies = await _movieRepository.GetAllMoviesAsync();

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
            var movie = await _movieRepository.GetMovieByIdAsync(id);
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

            await _movieRepository.AddMovieAsync(movie);
        }


        public async Task UpdateMovieAsync(int id, UpdateMovieDTO movieDto)
        {

            if (movieDto == null)
            {
                throw new ArgumentNullException(nameof(movieDto));
            }
            if (string.IsNullOrWhiteSpace(movieDto.Title))
            {
                throw new ArgumentException("Movie title cannot be null or empty.", nameof(movieDto.Title));
            }
            if (movieDto.ReleaseYear < 0)
            {
                throw new ArgumentException("Movie release year cannot be negative.", nameof(movieDto.ReleaseYear));
            }
            if (movieDto.ReleaseYear > DateTime.Now.Year)
            {
                throw new ArgumentException("Movie release year cannot be from future.", nameof(movieDto.ReleaseYear));
            }
            if (movieDto.StudioId <= 0)
            {
                throw new ArgumentException("Movie studio ID must be a positive integer.", nameof(movieDto.StudioId));
            }

            var movie = new Movie
            {
                Title = movieDto.Title,
                ReleaseYear = movieDto.ReleaseYear,
                StudioId = movieDto.StudioId
            };



            await _movieRepository.UpdateMovieAsync(id, movie);
            await _unitOfWork.SaveChangesAsync();
        }



        public async Task DeleteMovieAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Movie ID must be a positive integer.", nameof(id));
            }
            await _movieRepository.DeleteMovieAsync(id);
            await _unitOfWork.SaveChangesAsync();

        }
    }
}