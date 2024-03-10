using System.ComponentModel.DataAnnotations;

namespace Comp1640.Models
{
    public class Faculty
    {
        [Key]
        public int FacultyID { get; set; }

        public string Name { get; set; }
        public ICollection<Article> Articles { get; set; }
    }
}
