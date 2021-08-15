using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace SocialMediumForMusicians.Data.Models
{
    public class Musician : User
    {
        [Required]
        public decimal Price { get; set; }

        public string LongDescription { get; set; }

        [InverseProperty("Target")]
        public List<Review> Reviews { get; set; }

        public List<string> Instruments { get; set; }
    }
}
