using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Comp1640_Final.Models
{
    public class Dislike
    {
        [Key]
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        public Guid? ArticleId { get; set; }
        [ForeignKey("ArticleId")]
        public Article Article { get; set; }

        public Guid? CommentId { get; set; }
        [ForeignKey("CommentId")]
        public Comment Comment { get; set; }
    }
}
