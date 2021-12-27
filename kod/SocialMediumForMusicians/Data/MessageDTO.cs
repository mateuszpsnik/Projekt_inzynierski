using System;

namespace SocialMediumForMusicians.Data
{
    public class MessageDTO
    {
        public string Id { get; set; }
        public string AuthorId { get; set; }
        public string RecipentId { get; set; }
        public string Content { get; set; }
        public bool Read { get; set; }
        public DateTime SentAt { get; set; }
        public string AuthorName { get; set; }
        public string AuthorImgFilename { get; set; }
    }
}
