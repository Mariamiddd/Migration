using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MovieDomain.DTOs;
using MovieDomain.Entities;
using MovieDomain.Interfaces;
using MovieInfrastructure.Data;
using MovieInfrastructure.Repositories;
using MovieService.Implementations;
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
            services.AddScoped<IActorRepository, ActorRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IActorService, ActorService>();

            var serviceProvider = services.BuildServiceProvider();

            var movieService = serviceProvider.GetRequiredService<IMovieService>();
            var actorService = serviceProvider.GetRequiredService<IActorService>();

            var dbContext = serviceProvider.GetRequiredService<MovieDbContext>();





            var actorsWithMovies = await dbContext.Actors
                .Include(a => a.Movies)
                .ToListAsync();
            foreach (var item in actorsWithMovies)
            {
                Console.Write($"{item.FirstName} {item.LastName}");
                foreach (var movie in item.Movies)
                {
                    Console.Write($" - {movie.Title}");
                }
                Console.WriteLine();
            }


            //test
            //  add studio
            var studio = new Studio { Name = "Universal Pictures", CountryId = 1 };
            dbContext.Studios.Add(studio);
            await dbContext.SaveChangesAsync();

            // create movie
            var createMovieDto = new CreateMovieDTO
            {
                Title = "singin in the rain",
                ReleaseYear = 1952,
                StudioId = studio.Id
            };
            await movieService.AddMovieAsync(createMovieDto);
            Console.WriteLine("--- movie added successfully! ---");

            // find last movie in the database
            var allMovies = await movieService.GetAllMovies();
            var lastMovie = allMovies.LastOrDefault();

            if (lastMovie != null)
            {
                Console.WriteLine($"last movie in the database: ID: {lastMovie.Id}, Title: {lastMovie.Title}, Studio: {lastMovie.StudioName}");
            }
        }
    }
}