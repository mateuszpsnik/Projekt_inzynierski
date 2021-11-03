using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace SocialMediumForMusicians.Data.Models
{
    public enum MusicianType
    {
        Teacher,
        Jamming,
        Session
    }

    [Table("Musicians")]
    public class Musician : User
    {
        [Required]
        [Column(TypeName = "decimal(6,2)")]
        public decimal Price { get; set; }

        public string LongDescription { get; set; }

        public List<Review> Reviews { get; set; }

        public List<string> Instruments { get; set; }

        public List<MusicianType> Types { get; set; }

        public List<EmailMessage> EmailMessages { get; set; }
    }
}
