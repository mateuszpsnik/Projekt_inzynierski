using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace SocialMediumForMusicians.Data.Models
{
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

        public List<Meeting> Meetings { get; set; }

        [InverseProperty("Recipent")]
        public List<Message> InMessages { get; set; }

        [InverseProperty("Author")]
        public List<Message> OutMessages { get; set; }

        [InverseProperty("Author")]
        public List<Review> MyReviews { get; set; }

        public List<int> FavouriteMusiciansIds { get; set; }
    }
}
