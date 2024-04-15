using Comp1640_Final.Data;
using Comp1640_Final.DTO.Request;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;

namespace Comp1640_Final.Services
{
    public interface IEmailService 
    {
        Task<bool> SendEmail(EmailDTO emailDTO);

    }
    public class EmailService: IEmailService
    {
        public EmailService()
        {
        }
        public async Task<bool> SendEmail(EmailDTO emailDTO)
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

                return true;
            }
        }

    }
}
