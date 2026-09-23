using System;
using System.Collections.Generic;
using System.Text;

namespace MovieDomain.DTOs
{
    public class ActorDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ICollection<string> MvoieTiles { get; set; } = new List<string>();
    }
}
