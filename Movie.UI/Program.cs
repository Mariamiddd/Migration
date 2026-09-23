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

            // create a new studio and add it to the database
            var studio = new Studio { Name = "MGM", CountryId = 1 }; 
            dbContext.Studios.Add(studio);
            await dbContext.SaveChangesAsync();

            var st = await dbContext.Studios.FirstOrDefaultAsync(s => s.Name == "MGM");

            // create movie
            var movieDto = new CreateMovieDTO
            {
                Title = "Singin' in the Rain",
                ReleaseYear = 1952,
                StudioId = st.Id
            };
            await movieService.AddMovieAsync(movieDto);

            //create actor from the movie
            var actorDto = new CreateActorDTO
            {
                FirstName = "Gene",
                LastName = "Kelly"
            };
            await actorService.AddActorAsync(actorDto);

            Console.WriteLine("film and actor created");

            // take the movie and actor from the database
            var film = await dbContext.Movies
                .FirstAsync(m => m.Title == "Singin' in the Rain");

            var actor = await dbContext.Actors
                .FirstAsync(a => a.FirstName == "Gene" && a.LastName == "Kelly");

            // connect novie - actor
            var updateActorDTO = new UpdateActorMovieDTO
            {
                MovieIds = new List<int> { film.Id }
            };

            await actorService.UpdateActorMoviesAsync(actor.Id, updateActorDTO);

            Console.WriteLine("film and actor connected successfully.");



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