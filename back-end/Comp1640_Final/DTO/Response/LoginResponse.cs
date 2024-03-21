using System.ComponentModel.DataAnnotations;

namespace Comp1640_Final.DTO.Response
{
    public class LoginResponse
    {
        public string? Id { get; set; }

        public string FirstName { get; set; }
        public byte[] Avatar { get; set; }

        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string[] Roles { get; set; }
        public int FacultyId { get; set; }

        public string Jwt_token { get; set; }

    }
}
