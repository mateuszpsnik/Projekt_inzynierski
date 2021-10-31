namespace SocialMediumForMusicians.Data
{
    public class ReviewsListDTO
    {
        public int Id { get; set; }
        public int Rate { get; set; }
        public string Description { get; set; }
        public string AuthorName { get; set; }
        public string AuthorProfilePicFilename { get; set; }
        public string TargetId { get; set; }
        public string TargetProfilePicFilename { get; set; }        
    }
}
