using System;
using System.ComponentModel.DataAnnotations;

namespace SocialMediumForMusicians.Data.Models
{
    public class EmailMessage
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string AuthorEmail { get; set; }

        public string RecipentId { get; set; }

        [Required]
        public Musician Recipent { get; set; }

        [Required]
        public string Content { get; set; }

        public bool Read { get; set; }

        [Required]
        public DateTime SentAt { get; set; }
    }
}
