using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialMediumForMusicians.Data.Models
{
    [Table("Meetings")]
    public class Meeting
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string HostId { get; set; }

        [Required]
        public string GuestId { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        public User Host { get; set; }

        public User Guest { get; set; }

        public string Notes { get; set; }

        public bool Accepted { get; set; }
    }
}
