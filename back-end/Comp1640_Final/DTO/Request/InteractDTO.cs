namespace Comp1640_Final.DTO.Request
{
    public class InteractDTO
    {
        public int? Id { get; set; }
        public string UserId { get; set; }
        public Guid? ArticleId { get; set; }
        public Guid? CommentId { get; set; }
    }
}
