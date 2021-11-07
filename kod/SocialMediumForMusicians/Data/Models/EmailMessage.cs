using System;
using System.ComponentModel.DataAnnotations;

namespace SocialMediumForMusicians.Data.Models
{
    public class EmailMessage
    {
        [Key]
        public int Key { get; set; }

        [Required]
        public string Id { get; set; }

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
