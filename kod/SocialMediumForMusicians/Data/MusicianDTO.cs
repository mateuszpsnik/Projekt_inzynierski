using SocialMediumForMusicians.Data.Models;
using System;
using System.Linq;
using System.Collections.Generic;

namespace SocialMediumForMusicians.Data
{
    public class MusicianDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ProfilePicFilename { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public decimal Price { get; set; }
        public List<string> Instruments { get; set; }
        public double AvgScore { get; set; }
        public int FullStars => (int)Math.Round(AvgScore, 3, MidpointRounding.AwayFromZero);

        private double fractionalPart => AvgScore - Math.Floor(AvgScore);
        public bool HalfStar => fractionalPart > 0.000 && fractionalPart <= 0.500;
    }
}
