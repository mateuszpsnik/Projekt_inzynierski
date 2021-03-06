using System;

namespace SocialMediumForMusicians.Data
{
    public class ReviewsListDTO
    {   
        public string Id { get; set; }
        public int Rate { get; set; }
        public string Description { get; set; }
        public string AuthorName { get; set; }
        public string AuthorProfilePicFilename { get; set; }
        public string TargetId { get; set; }
        public string TargetProfilePicFilename { get; set; }
        public string TargetName { get; set; }
        public DateTime SentAt { get; set; }
        public string AuthorId { get; set; }
    }
}
