using MovieDomain.Entities;
using MovieDomain.Interfaces;
using MovieInfrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieInfrastructure.Repositories
{
    public class ActorRepository : IActorRepository
    {

        private readonly MovieDbContext _movieDbContext;
        public ActorRepository(MovieDbContext movieDbContext)
        {
            _movieDbContext = movieDbContext;
        }

        public async Task AddActorAsync(Actor actor)
        {
            await _movieDbContext.Actors.AddAsync(actor);
        }


        public async Task<ICollection<Actor>> GetAllActorsAsync()
        {
            return await _movieDbContext.Actors
                .Include(a => a.Movies)
                .ToListAsync();
        }




        public async Task<Actor> GetActorByIdAsync(int id)
        {
            return await _movieDbContext.Actors
                .Include(a => a.Movies)
                .FirstOrDefaultAsync(a => a.Id == id);
        }





        public async Task UpdateActorAsync(int id, Actor actor)
        {
            var actorExists =
                await _movieDbContext.Actors
                .FirstOrDefaultAsync(a => a.Id == id);
            if (actorExists == null)
            {
                throw new ArgumentException("Actor not found");
            }
            actorExists.FirstName = actor.FirstName;
            actorExists.LastName = actor.LastName;
        }





        public async Task DeleteActorAsync(int id)
        {
            var actorExists =
                await _movieDbContext.Actors
                .FirstOrDefaultAsync(a => a.Id == id);
            if (actorExists == null)
            {
                throw new ArgumentException("Actor not found");
            }
            _movieDbContext.Actors.Remove(actorExists);
        }



        
        public async Task UpdateActorMoviesAsync(int actorId, ICollection<int> movieIds)
        {
            var actor = await _movieDbContext.Actors
                .Include(a => a.Movies)
                .FirstOrDefaultAsync(a => a.Id == actorId);

            if (actor == null)
            {
                throw new ArgumentException("Actor not found");
            }


            var movies = await _movieDbContext.Movies
                .Where(m => movieIds.Contains(m.Id))
                .ToListAsync();


            actor.Movies.Clear();

            foreach (var movie in movies)
            {
                actor.Movies.Add(movie);
            }


        }

    }
}