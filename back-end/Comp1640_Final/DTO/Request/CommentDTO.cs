namespace Comp1640_Final.DTO.Request
{
    public class CommentDTO
    {

        public string Content { get; set; }
        public string UserId { get; set; }
        public Guid ArticleId { get; set; }
        public bool? IsAnonymous { get; set; }

    }

    public class EditCommentDTO
    {
        public Guid Id { get; set; }

        public string Content { get; set; }

    }

    //public class ReplyDTO
    //{
    //    public Guid Id { get; set; }
    //    public string Content { get; set; }
    //    public DateTime CreateOn { get; set; }
    //    public string UserId { get; set; }
    //    public Guid ArticleId { get; set; }
    //    public Guid ParentCommentId { get; set; }
    //}
}
