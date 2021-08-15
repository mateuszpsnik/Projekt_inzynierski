using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialMediumForMusicians.Data.Models
{
    public class Meeting
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [ForeignKey(nameof(User))]
        public int HostId { get; set; }

        [ForeignKey(nameof(User))]
        public int GuestId { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [Required]
        public User Host { get; set; }

        [Required]
        public User Guest { get; set; }

        public string Notes { get; set; }
    }
}
