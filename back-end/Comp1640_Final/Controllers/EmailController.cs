using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using Comp1640_Final.DTO;

namespace Comp1640_Final.Controllers
{
    [Route("api/email")]
    [ApiController]
    public class EmailController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> SendEmail([FromBody] EmailDTO emailDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var mail = "cmuniversity420@gmail.com\r\n";
                var pass = "avkl cbmi bscq orfn";
                // Create and configure the SMTP client
                using (SmtpClient client = new SmtpClient("smtp.gmail.com"))
                {
                    client.Port = 587;
                    //client.UseDefaultCredentials = false;
                    client.EnableSsl = true;
                    client.Credentials = new System.Net.NetworkCredential(mail, pass);

                    // Create the email message
                    MailMessage message = new MailMessage();
                    message.From = new MailAddress("cmuniversity420@gmail.com\r\n");
                    message.To.Add(new MailAddress(emailDTO.Email));
                    message.Subject = emailDTO.Subject;
                    message.Body = emailDTO.Body;

                    // Send the email
                    client.Send(message);

                    return Ok("Email sent successfully");
                }
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, "An error occurred while sending the email.");
            }
        }
    }
}
