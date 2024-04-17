using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Comp1640_Final.Models
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        public string UserInteractionId { get; set; }
        [ForeignKey("UserInteractionId")]
        public ApplicationUser UserInteraction { get; set; }
        public Guid? ArticleId { get; set; }
        [ForeignKey("ArticleId")]
        public Article Article { get; set; }

        public Guid? CommentId { get; set; }
        [ForeignKey("CommentId")]
        public Comment Comment { get; set; }
        public string Message { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool? IsAnonymous { get; set; }
        public bool IsRead { get; set; }
    }
}
