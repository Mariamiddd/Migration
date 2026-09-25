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


            // create studio if it doesn't existსხქ

            var st = await dbContext.Studios.FirstOrDefaultAsync(s => s.Name == "MGM");
            if (st == null)
            {
                st = new Studio { Name = "MGM", CountryId = 1 };
                dbContext.Studios.Add(st);
                await dbContext.SaveChangesAsync();
                Console.WriteLine("Studio created.");
            }

            var actorsWithMovies = await dbContext.Actors
                .Include(a => a.Movies)
                .ToListAsync();
            foreach (var item in actorsWithMovies)
            {
                Console.Write($"{item.FirstName} {item.LastName}");
                foreach (var m in item.Movies)
                {
                    Console.Write($" - {m.Title}");
                }
                Console.WriteLine("");
            }

            //create movie if it doesn't exist
            var movie = await dbContext.Movies.FirstOrDefaultAsync(m => m.Title == "Singin' in the Rain");
            if (movie == null)
            {
                var movieDto = new CreateMovieDTO
                {
                    Title = "Singin' in the Rain",
                    ReleaseYear = 1952,
                    StudioId = st.Id
                };
                await movieService.AddMovieAsync(movieDto);
                // retrieve the movie from the database after adding it
                movie = await dbContext.Movies.FirstAsync(m => m.Title == "Singin' in the Rain");
                Console.WriteLine("Movie created.");
            }

            //add actor if it doesn't exist

            var actor = await dbContext.Actors.FirstOrDefaultAsync(a => a.FirstName == "Gene" && a.LastName == "Kelly");
            if (actor == null)
            {
                var actorDto = new CreateActorDTO
                {
                    FirstName = "Gene",
                    LastName = "Kelly"
                };
                await actorService.AddActorAsync(actorDto);

                actor = await dbContext.Actors.FirstAsync(a => a.FirstName == "Gene" && a.LastName == "Kelly");
                Console.WriteLine("Actor created.");
            }

            //check the connection between actor and movie
            var actorWithMovies = await dbContext.Actors
                .Include(a => a.Movies)
                .FirstAsync(a => a.Id == actor.Id);

            if (!actorWithMovies.Movies.Any(m => m.Id == movie.Id))
            {
                var updateActorDTO = new UpdateActorMovieDTO
                {
                    MovieIds = new List<int> { movie.Id }
                };
                await actorService.UpdateActorMoviesAsync(actor.Id, updateActorDTO);
                Console.WriteLine("Film and actor connected successfully.");
            }


            // add actor Donald
            var donald = await dbContext.Actors.FirstOrDefaultAsync(a => a.FirstName == "Donald" && a.LastName == "O'Connor");
            if (donald == null)
            {
                var donaldDto = new CreateActorDTO { FirstName = "Donald", LastName = "O'Connor" };
                await actorService.AddActorAsync(donaldDto);
                donald = await dbContext.Actors.FirstAsync(a => a.FirstName == "Donald" && a.LastName == "O'Connor");
                Console.WriteLine("Donald created.");
            }

            // create movie
            var parisMovie = await dbContext.Movies.FirstOrDefaultAsync(m => m.Title == "An American in Paris");
            if (parisMovie == null)
            {
                var parisDto = new CreateMovieDTO { Title = "An American in Paris", ReleaseYear = 1951, StudioId = st.Id };
                await movieService.AddMovieAsync(parisDto);
                Console.WriteLine("Movie 'An American in Paris' created.");
            }

            // check and add movie
            var townMovie = await dbContext.Movies.FirstOrDefaultAsync(m => m.Title == "On the Town");
            if (townMovie == null)
            {
                var townDto = new CreateMovieDTO { Title = "On the Town", ReleaseYear = 1949, StudioId = st.Id };
                await movieService.AddMovieAsync(townDto);
                Console.WriteLine("Movie 'On the Town' created.");
            }

            // connect donald to the movie
            var donaldWithMovies = await dbContext.Actors.Include(a => a.Movies).FirstAsync(a => a.Id == donald.Id);
            if (!donaldWithMovies.Movies.Any(m => m.Id == movie.Id))
            {
                var donaldUpdateDTO = new UpdateActorMovieDTO { MovieIds = new List<int> { movie.Id } };
                await actorService.UpdateActorMoviesAsync(donald.Id, donaldUpdateDTO);
                Console.WriteLine("Donald connected to 'Singin' in the Rain'.");
            }

            var paris = await dbContext.Movies.FirstAsync(m => m.Title == "An American in Paris");
            var gene = await dbContext.Actors.FirstAsync(a => a.FirstName == "Gene");

            // update movies forgnene
            var geneMovies = new UpdateActorMovieDTO
            {
                MovieIds = new List<int> { movie.Id, paris.Id }
            };
            await actorService.UpdateActorMoviesAsync(gene.Id, geneMovies);

            //////////////////////////////////////////////////////////////////
            ///რთული ქვერი ბრძანებები////

            //davaleba 1 test
            var searchedMovies = await movieService.SearchMoviesByStudioAsync(1950, "MGM", 1);

            Console.WriteLine("\n--- davaleba 1 search results ---");
            foreach (var m in searchedMovies)
            {
                Console.WriteLine($"- {m.Title} ({m.ReleaseYear})");
            }

            // davaleba 2 test
            var countryMovies = await movieService.SearchMoviesByCountryAsync("USA", 1900, 5);

            Console.WriteLine("\n--- davaleba 2 testing ---");
            foreach (var m in countryMovies)
            {
                Console.WriteLine($"- {m.Title} ({m.ReleaseYear}) - studio: {m.StudioName} - country: {m.CountryName}");
            }


            // davaleba 3 test
            var advancedMovies = await movieService.SearchMoviesAdvancedAsync(1800, 2026, "USA", "i", 1);

            Console.WriteLine("\n--- davaleba 3 testing ---");
            foreach (var m in advancedMovies)
            {
                Console.WriteLine($"- {m.Title} ({m.ReleaseYear}) - studio: {m.StudioName} - country: {m.CountryName}");
            }

        }

    }
}