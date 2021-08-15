using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialMediumForMusicians.Data.Models
{
    public class Message
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [ForeignKey(nameof(User))]
        public int AuthorId { get; set; }

        [ForeignKey(nameof(User))]
        public int RecipentId { get; set; }

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
