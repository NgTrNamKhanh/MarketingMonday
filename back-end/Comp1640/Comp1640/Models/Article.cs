using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Comp1640.Models
{
    public class Article
    {
        [Key]
        public Guid ArticleId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
        public DateTime Date { get; set; }

        [ForeignKey("Faculty")]
        public int FacultyID { get; set; }
        public Faculty Faculty { get; set; }
    }
}
