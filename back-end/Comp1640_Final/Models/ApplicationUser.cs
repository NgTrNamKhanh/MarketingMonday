using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Comp1640_Final.Models
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string FirstName { get; set; }

        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string LastName { get; set; }
        public string? AvatarImagePath { get; set; }
        public string? CloudAvatarImagePath { get; set; }

        [ForeignKey("FacultyId")]
        public int FacultyId { get; set; }
        public Faculty Faculty { get; set; }
    }
}
