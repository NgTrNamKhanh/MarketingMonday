namespace Comp1640_Final.DTO.Request
{
    public class ArticleInteractDTO
    {
        public string UserId { get; set; }
        public Guid ArticleId { get; set; }
    }
    public class CommentInteractDTO
    {
        public string UserId { get; set; }
        public Guid CommentId { get; set; }
    }
}
