namespace Comp1640_Final.DTO.Response
{
    public class ArticleResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime UploadDate { get; set; }
        public int FacultyId { get; set; }
        public byte[] StudentAvatar { get; set; }
        public string StudentId { get; set; }
        public string StudentName { get; set; }
        public List<byte[]> ImageBytes { get; set; }

        public int CommmentCount { get; set; }
        public int LikeCount { get; set; }
        public int DislikeCount { get; set; }
        public int ViewCount { get; set; }
        public bool IsLiked { get; set; }
        public bool IsDisliked { get; set; }
        public bool IsViewed { get; set; }

        //public string MarketingCoordinatorId { get; set; }
    }
    public class SubmissionResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime UploadDate { get; set; }
        public int FacultyId { get; set; }
        public byte[] StudentAvatar { get; set; }
        public string StudentName { get; set; }
        public int PublishStatusId { get; set; }
        public string CloudImagePath { get; set; }
        public string CloudDocPath { get; set; }

        public string CoordinatorComment { get; set; }

        public int CommmentCount { get; set; }
        public int LikeCount { get; set; }
        public int DislikeCount { get; set; }
        public bool IsLiked { get; set; }
        public bool IsDisliked { get; set; }
        public int ViewCount { get; set; }
        public bool IsViewed { get; set; }
        public List<byte[]> ImageBytes { get; set; }

        //public string MarketingCoordinatorId { get; set; }
    }
}
