using System;
using System.Collections.Generic;
using System.Text;

namespace MovieDomain.DTOs
{
    public class UpdateActorMovieDTO
    {
        public ICollection<int> MovieIds { get; set; } = new List<int>();
    }
}
