using System;
using System.Collections.Generic;
using System.Text;

namespace MovieDomain.DTOs
{
    public class UpdateMovieDTO
    {
        public string Title { get; set; } = string.Empty;
        public int ReleaseYear { get; set; }
        public int StudioId { get; set; }

    }

}
