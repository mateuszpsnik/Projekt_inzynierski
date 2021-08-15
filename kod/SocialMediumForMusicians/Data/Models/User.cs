using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace SocialMediumForMusicians.Data.Models
{
    [Table("Users")]
    public class User
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Name { get; set; }

        public string ProfilePicFilename { get; set; }

        public string Description { get; set; }

        public List<Meeting> HostedMeetings { get; set; }

        public List<Meeting> GuestMeetings { get; set; }

        public List<Message> InMessages { get; set; }

        public List<Message> OutMessages { get; set; }

        public List<Review> MyReviews { get; set; }

        public List<int> FavouriteMusiciansIds { get; set; }
    }
}
