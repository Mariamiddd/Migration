using System;
using System.Collections.Generic;
using System.Text;

namespace MovieDomain.Entities
{
    public class Studio
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CountryId { get; set; }
        public Country Country { get; set; }
        public StudioDetails StudioDetails { get; set; }
        public ICollection<Movie> Movies { get; set; }
    }
}
