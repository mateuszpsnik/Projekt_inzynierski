using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialMediumForMusicians.Data.Models
{
    [Table("Messages")]
    public class Message
    {
        [Key]
        public int Key { get; set; }

        [Required]
        public string Id { get; set; }

        [Required]
        public string AuthorId { get; set; }

        [Required]
        public string RecipentId { get; set; }

        public User Author { get; set; }

        public User Recipent { get; set; }

        [Required]
        public string Content { get; set; }

        public bool Read { get; set; }

        [Required]
        public DateTime SentAt { get; set; }
    }
}
