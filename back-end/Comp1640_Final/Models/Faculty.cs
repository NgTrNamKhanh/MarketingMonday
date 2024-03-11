using System.ComponentModel.DataAnnotations;

namespace Comp1640_Final.Models
{
    public class Faculty
    {
        [Key]
        public int FacultyId { get; set; }

        public string Name { get; set; }
        public ICollection<Article> Articles { get; set; }
    }
}
