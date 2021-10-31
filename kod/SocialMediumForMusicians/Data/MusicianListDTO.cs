using SocialMediumForMusicians.Data.Models;
using System.Collections.Generic;

namespace SocialMediumForMusicians.Data
{
    public class MusiciansListDTO
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string ProfilePicFilename { get; set; }
        public List<string> Instruments { get; set; }
        public List<MusicianType> Types { get; set; }
        public double AvgScore { get; set; }
    }
}
