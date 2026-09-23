using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MovieDomain.DTOs;
using MovieDomain.Entities;
using MovieDomain.Interfaces;
using MovieInfrastructure.Data;
using MovieInfrastructure.Repositories;
using MovieService.Interfaces;

namespace Movie.UI
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            //di container setup
            var services = new ServiceCollection();

            services.AddDbContext<MovieDbContext>();
            services.AddScoped<IMovieRepository, MovieRepository>();
            services.AddScoped<IMovieService, MovieService.Implementations.MovieService>();

            var serviceProvider = services.BuildServiceProvider();

            var movieService = serviceProvider.GetRequiredService<IMovieService>();

            var dbContext = serviceProvider.GetRequiredService<MovieDbContext>();

            var studio = new Studio
            {
                Name = "Warner Bros.",
                CountryId = 1
            };

            dbContext.Studios.Add(studio);
            await dbContext.SaveChangesAsync();

            var createMovieDto = new CreateMovieDTO
            {
                Title = "Inception",
                ReleaseYear = 2010,
                StudioId = 1 
            };

            await movieService.AddMovieAsync(createMovieDto);

         
            await dbContext.SaveChangesAsync();

            var movieById = await movieService.GetMovieById(1);
            Console.WriteLine($"Retrieved Movie: Title: {movieById.Title}, Release Year: {movieById.ReleaseYear}, Studio: {movieById.StudioName}");

            var movies = await movieService.GetAllMovies();

            foreach (var movie in movies)
            {
                Console.WriteLine($"Title: {movie.Title}, Release Year: {movie.ReleaseYear}, Studio: {movie.StudioName}");
            }



            // test update
            var updateMovieDto = new UpdateMovieDTO
            {
                Id = 1, // update the movie with ID 1
                Title = "Inception - Updated",
                ReleaseYear = 2012,
                StudioId = 1
            };

            await movieService.UpdateMovieAsync(updateMovieDto);
            Console.WriteLine("\n--- Movie Updated ---");
            var updatedMovie = await movieService.GetMovieById(1);
            Console.WriteLine($"Title: {updatedMovie.Title}, Release Year: {updatedMovie.ReleaseYear}");

            // test delete
            await movieService.DeleteMovieAsync(1);
            Console.WriteLine("\n--- Movie Deleted ---");

            var remainingMovies = await movieService.GetAllMovies();
            Console.WriteLine($"Remaining movies count: {remainingMovies.Count}");
        }
    }
}