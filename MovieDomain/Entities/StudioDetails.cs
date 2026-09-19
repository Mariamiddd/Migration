using System;
using System.Collections.Generic;
using System.Text;

namespace MovieDomain.Entities
{
    public class StudioDetails
    {
        public int Id { get; set; }
        public string LicenseNumber { get; set; }
        public int StudioId { get; set; }
        public Studio Studio { get; set; }

    }
}
