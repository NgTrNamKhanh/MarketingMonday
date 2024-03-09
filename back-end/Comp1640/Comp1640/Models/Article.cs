using System.ComponentModel.DataAnnotations;

namespace Comp1640.Models
{
    public class Article
    {
        [Key]
        public Guid ArticleId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
    }
}
