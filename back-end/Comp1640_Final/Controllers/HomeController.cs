using AutoMapper;
using Comp1640_Final.DTO.Response;
using Comp1640_Final.IServices;
using Comp1640_Final.Models;
using Comp1640_Final.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Comp1640_Final.Controllers
{
    [Route("api")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private static IWebHostEnvironment _webHostEnvironment;
        private readonly IUserService _userService;
        public HomeController(IAuthService authService, UserManager<ApplicationUser> userManager, IMapper mapper, IConfiguration configuration, IWebHostEnvironment webHostEnvironment, IUserService userService)
        {
            _authService = authService;
            _userManager = userManager;
            _mapper = mapper;
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
            _userService = userService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(string email, string passWord)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await _authService.Login(email, passWord))
            {
                var identityUser = await _userManager.FindByEmailAsync(email);
                var roles = await _userManager.GetRolesAsync(identityUser);

                var token = CreateToken(identityUser, roles[0]);
                var cloudUserImage = await _userService.GetCloudinaryAvatarImagePath(identityUser.Id); // Await the method call

                // If imageBytes is null, read the default image file
                if (cloudUserImage == null)
                {
                    var defaultImageFileName = "http://res.cloudinary.com/dizeyf6y0/image/upload/v1712075739/pxfrfocprhnsriutmg3r.jpg";
                    cloudUserImage = defaultImageFileName;
                }

                var accountDto = new LoginResponse
                {
                    Id = identityUser.Id,
                    FirstName = identityUser.FirstName,
                    LastName = identityUser.LastName,
                    PhoneNumber = identityUser.PhoneNumber,
                    Email = identityUser.Email,
                    Roles = roles.ToArray(),
                    FacultyId = identityUser.FacultyId,
                    Avatar = cloudUserImage,
                    Jwt_token = token
                };

                // Set cookie
                Response.Cookies.Append("CMU-user", JsonConvert.SerializeObject(accountDto), new CookieOptions
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.Now.AddDays(1)
                });

                return Ok(accountDto);
            }

            return BadRequest("Email or password is not correct");
        }


        private string CreateToken(ApplicationUser user, string role)
        {
            List<Claim> claims = new List<Claim>{
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:Key").Value));
            //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(user.UserName));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                issuer: _configuration.GetSection("Jwt:Issuer").Value,
                audience: _configuration.GetSection("Jwt:Audience").Value,
                signingCredentials: creds
             );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;

        }

    }
}
