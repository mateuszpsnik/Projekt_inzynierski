using System;
using System.ComponentModel.DataAnnotations;

namespace SocialMediumForMusicians.Data.Models
{
    public class EmailMessage
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string AuthorEmail { get; set; }

        [Required]
        public string RecipentId { get; set; }

        public Musician Recipent { get; set; }

        [Required]
        public string Content { get; set; }

        public bool Read { get; set; }

        [Required]
        public DateTime SentAt { get; set; }
    }
}
