using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialMediumForMusicians.Data.Models
{
    [Table("Messages")]
    public class Message
    {
        [Key]
        [Required]
        public int Id { get; set; }

        public string AuthorId { get; set; }

        public string RecipentId { get; set; }

        [Required]
        public User Author { get; set; }

        [Required]
        public User Recipent { get; set; }

        [Required]
        public string Content { get; set; }

        public bool Read { get; set; }

        [Required]
        public DateTime SentAt { get; set; }
    }
}
