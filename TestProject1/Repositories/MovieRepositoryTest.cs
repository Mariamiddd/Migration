using Microsoft.EntityFrameworkCore;
using MovieDomain.Entities;
using MovieInfrastructure.Data;
using MovieInfrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;


namespace TestProject1.Repositories
{
    public class MovieRepositoryTest
    {
        private MovieDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<MovieDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new MovieDbContext(options);
        }

        [Fact]
        public async Task GetAllMoviesAsync_ReturnsAllMovies()
        {
            // 
            var context = CreateContext();

            var country = new Country { Id = 1, countryName = "USA" };
            var studio = new Studio { Id = 1, Name = "Warner Bros", CountryId = country.Id };

            var movie1 = new Movie { Id = 1, Title = "Inception", ReleaseYear = 2010, StudioId = studio.Id };
            var movie2 = new Movie { Id = 2, Title = "Interstellar", ReleaseYear = 2014, StudioId = studio.Id };

            // add everything
            context.Countries.Add(country);
            context.Studios.Add(studio);
            context.Movies.AddRange(movie1, movie2);

            //save
            await context.SaveChangesAsync();

            // system under test
            var sut = new MovieRepository(context);

            
            // Act
            var result = await sut.GetAllMoviesAsync();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, m => m.Title == "Inception" && m.ReleaseYear == 2010);

            var singleFilm = result.First(m => m.Title == "Inception");
            Assert.Equal(1, singleFilm.Id);

            Assert.NotNull(singleFilm.Studio);
            Assert.Equal(1, singleFilm.Studio.Id);
            Assert.Equal("Warner Bros", singleFilm.Studio.Name);
            Assert.Equal(1, singleFilm.Studio.CountryId);
        
        }
    }
}