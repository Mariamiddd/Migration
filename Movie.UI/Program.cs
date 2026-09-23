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
            services.AddScoped<IActorService, ActorService>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            var serviceProvider = services.BuildServiceProvider();

            var movieService = serviceProvider.GetRequiredService<IMovieService>();
            var actorService = serviceProvider.GetRequiredService<IActorService>();

            var dbContext = serviceProvider.GetRequiredService<MovieDbContext>();





            //var studio = new Studio
            //{
            //    Name = "Warner Bros.",
            //    CountryId = 1
            //};


            /////
            //servises.AddDbContext<MovieDbContext>();
            //servises.AddScoped<IMovieRepository, MovieRepository>();
            //servises.AddScoped<IMovieService, MovieService>();

            //servises.AddScoped<IActorRepository, ActorRepository>();
            //servises.AddScoped<IActorService, ActorService>();


            //servises.AddScoped<IUnitOfWork, UnitOfWork>();




            //var serviceProvider = servises.BuildServiceProvider();

            //var movieService = serviceProvider.GetRequiredService<IMovieService>();
            //var actorService = serviceProvider.GetRequiredService<IActorService>();

            ////

            //dbContext.Studios.Add(studio);
            //await dbContext.SaveChangesAsync();

            //var createMovieDto = new CreateMovieDTO
            //{
            //    Title = "Inception",
            //    ReleaseYear = 2010,
            //    StudioId = studio.Id
            //};

            //await movieService.AddMovieAsync(createMovieDto);


            //var movieById = await movieService.GetMovieById(1);
            //Console.WriteLine($"Retrieved Movie: Title: {movieById.Title}, Release Year: {movieById.ReleaseYear}, Studio: {movieById.StudioName}");

            //var movies = await movieService.GetAllMovies();

            //foreach (var movie in movies)
            //{
            //    Console.WriteLine($"Title: {movie.Title}, Release Year: {movie.ReleaseYear}, Studio: {movie.StudioName}");
            //}



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
        }
    }
}