namespace Comp1640_Final.DTO
{
    public class CommentDTO
    {
        public Guid Id { get; set; }

        public string Content { get; set; }
        public DateTime CreateOn { get; set; }
        public string UserId { get; set; }
        public Guid ArticleId { get; set; }
        public Guid? ParentCommentId { get; set; }

    }

    public class ReplyDTO
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public DateTime CreateOn { get; set; }
        public string UserId { get; set; }
        public Guid ArticleId { get; set; }
        public Guid ParentCommentId { get; set; }
    }
}
