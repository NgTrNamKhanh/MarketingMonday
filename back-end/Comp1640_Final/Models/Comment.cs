using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Comp1640_Final.Models
{
    public class Comment
    {
        [Key]
        public Guid Id { get; set; }

        public string Content { get; set; }

        public DateTime CreateOn { get; set; } = DateTime.UtcNow;

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        public Guid ArticleId { get; set; }
        [ForeignKey("ArticleId")]
        public Article Article { get; set; }

        public Guid? ParentCommentId { get; set; }
        [ForeignKey("ParentCommentId")]
        public Comment ParentComment { get; set; }

        public ICollection<Comment> Replies { get; set; }

    }
}
