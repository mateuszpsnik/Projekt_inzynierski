using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialMediumForMusicians.Data.Models
{
    [Table("Reviews")]
    public class Review
    {
        [Key]
        [Required]
        public int Id { get; set; }

        public string AuthorId { get; set; }

        public string TargetId { get; set; }

        [Required]
        public int Rate { get; set; }

        [Required]
        public User Author { get; set; }

        [Required]
        public Musician Target { get; set; }

        public string Description { get; set; }
    }
}
