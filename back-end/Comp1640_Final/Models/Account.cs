using System.ComponentModel.DataAnnotations.Schema;

namespace Comp1640_Final.Models
{
    public class Account
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Role { get; set; }
        [NotMapped]
        public IFormFile AvatarImageFile { get; set; }
        public string? AvatarImagePath { get; set; }

        public int FacultyId { get; set; }
    }

    public class CreateAccountDTO
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Role { get; set; }
        [NotMapped]
        public IFormFile? AvatarImageFile { get; set; }
        public int FacultyId { get; set; }
    }

    public class EditAccountDTO
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Role { get; set; }
        public int FacultyId { get; set; }
    }
}
