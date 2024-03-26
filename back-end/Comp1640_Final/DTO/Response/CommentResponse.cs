namespace Comp1640_Final.DTO.Response
{
    public class CommentResponse
    {
        public Guid Id { get; set; }

        public string Content { get; set; }
        public DateTime CreateOn { get; set; }
        public Guid ArticleId { get; set; }

        public UserComment UserComment { get; set; }
        public int LikesCount { get; set; }
        public int DislikesCount { get; set; }
        public bool IsLiked { get; set; }
        public bool IsDisliked { get; set; }
        public string ParentCommentId { get; set; }
        public bool  hasReplies { get; set; }

    }
    public class UserComment 
    {
        public string Id { get; set; }
        public byte[] UserAvatar { get; set; }
        public string FirstName { get; set; }
		public string LastName { get; set; }

	}
}
