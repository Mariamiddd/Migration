using MovieDomain.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieService.Interfaces
{
    public interface IActorService
    {
        Task AddActorAsync(CreateActorDTO actorDto);
        Task<ActorDTO> GetActorAsync(int id);
        Task<ICollection<ActorDTO>> GetAllActorsAsync();
        Task UpdateActorAsync(int id, UpdateActorDTO actorDto);
        Task UpdateActorMoviesAsync(int actorId, UpdateActorMovieDTO updateActorMovieDto);
    }
}
