using System;

namespace SocialMediumForMusicians.Data
{
    public class EmailMessageDTO
    {
        public string Id { get; set; }
        public string AuthorEmail { get; set; }
        public string RecipentId { get; set; }
        public string Content { get; set; }
        public bool Read { get; set; }
        public DateTime SentAt { get; set; }
    }
}
