namespace Comp1640_Final.DTO.Response
{
    public class CommentResponse
    {
        public Guid Id { get; set; }

        public string Content { get; set; }
        public DateTime CreateOn { get; set; }
        public Guid ArticleId { get; set; }

        public UserComment UserComment { get; set; }
        public Guid ParentCommentId { get; set; }

    }
    public class UserComment 
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}
