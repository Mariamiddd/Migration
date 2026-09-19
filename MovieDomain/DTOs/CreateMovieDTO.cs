using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace MovieDomain.DTOs
{
    public class CreateMovieDTO
    {
        public string Title { get; set; }   
        public int ReleaseYear { get; set; }
        public int StudioId { get; set; }
    }
}
