using System.ComponentModel.DataAnnotations;

namespace Comp1640.Models
{
    public class Faculty
    {
        [Key]
        public Guid FacultyId { get; set; }

        public string Name { get; set; }
    }
}
