using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace SocialMediumForMusicians.Data.Models
{
    [Table("Reports")]
    public class Report
    {
        [Key]
        public int Key { get; set; }

        [Required]
        public string Id { get; set; }

        [Required]
        public string UserId { get; set; }

        public User User { get; set; }

        [Required]
        public string Justification { get; set; }
    }
}
