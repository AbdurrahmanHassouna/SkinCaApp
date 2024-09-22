using System.Net.Mail;
using System.Net;
using SkinCaApp.Authorization;
using Microsoft.Extensions.Options;
namespace SkinCaApp.Services
{
    public class EmailSender:IEmailSender
    {
        private readonly NetworkSecrets network;
        public EmailSender(IOptions<NetworkSecrets> options) { 
            network = options.Value;
        }
        public Task SendEmailAsync(string email,string subject,string message)
        {
            var stmpClient = new SmtpClient("smtp-mail.outlook.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(network.Email,network.Password)
            };
            
            return  stmpClient.SendMailAsync(
                new MailMessage(
                    from: network.Email,
                    to:email,
                    subject,
                    message
                    ));
        }
    }
}
