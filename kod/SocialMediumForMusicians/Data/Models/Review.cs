using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialMediumForMusicians.Data.Models
{
    [Table("Reviews")]
    public class Review
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string AuthorId { get; set; }

        [Required]
        public string TargetId { get; set; }

        [Required]
        public int Rate { get; set; }

        public User Author { get; set; }

        public Musician Target { get; set; }

        public string Description { get; set; }

        [Required]
        public DateTime SentAt { get; set; }
    }
}
