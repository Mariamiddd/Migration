using System;
using System.Collections.Generic;
using System.Text;

namespace MovieDomain.DTOs
{
    public class MovieDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int ReleaseYear { get; set; }
        public string StudioName { get; set; }
        public string CountryName { get; set; }


        public override string? ToString()
        {
            return $"MovieDTO: Id={Id}, Title={Title}, ReleaseYear={ReleaseYear}, StudioName={StudioName}, CountryName={CountryName}";
        }
    }

}
