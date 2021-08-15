using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialMediumForMusicians.Data.Models
{
    public class Review
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [ForeignKey(nameof(User))]
        public int AuthorId { get; set; }

        [ForeignKey(nameof(Musician))]
        public int TargerId { get; set; }

        [Required]
        public int Rate { get; set; }

        [Required]
        public User Author { get; set; }

        [Required]
        public Musician Target { get; set; }

        public string Description { get; set; }
    }
}
