using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace SocialMediumForMusicians.Data.Models
{
    public class User : IdentityUser
    {
        public bool IsMusician { get; set; }

        public string Name { get; set; }

        public string ProfilePicFilename { get; set; }

        public string Description { get; set; }

        public List<Meeting> HostedMeetings { get; set; }

        public List<Meeting> GuestMeetings { get; set; }

        public List<Message> InMessages { get; set; }

        public List<Message> OutMessages { get; set; }

        public List<Review> MyReviews { get; set; }

        public List<string> FavouriteMusiciansIds { get; set; }
    }
}
