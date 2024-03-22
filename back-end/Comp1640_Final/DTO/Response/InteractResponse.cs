namespace Comp1640_Final.DTO.Response
{
    public class InteractResponse
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }

        public string UserId { get; set; }
        public Guid? ArticleId { get; set; }
        public Guid? CommentId { get; set; }
    }
}
