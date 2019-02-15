using System;

namespace BHJet_DTO.Area
{
 
    public class AreasFiltroDTO
    {
        public Area[] Area { get; set; }
    }

 
    public class Area
    {
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}
