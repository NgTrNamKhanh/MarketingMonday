using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Comp1640_Final.Models
{
    public class Article
    {
        [Key]
        public Guid ArticleId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
        public DateTime Date { get; set; }

        [ForeignKey("FacultyId")]
        public int FacultyId { get; set; }
        public Faculty Faculty { get; set; }
    }
}
