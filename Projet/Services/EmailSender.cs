using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using Projet.Models;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Projet.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailConfiguration _emailConfiguration;

        public EmailSender(IOptions<EmailConfiguration> emailConfiguration)
        {
            _emailConfiguration = emailConfiguration.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            using (var smtpClient = new SmtpClient(_emailConfiguration.SmtpServer))
            {
                smtpClient.Port = _emailConfiguration.SmtpPort;
                smtpClient.Credentials = new NetworkCredential(_emailConfiguration.SmtpUsername, _emailConfiguration.SmtpPassword);
                smtpClient.EnableSsl = _emailConfiguration.EnableSsl; // Utilisation de EnableSsl ici

                var message = new MailMessage(_emailConfiguration.FromEmail, email)
                {
                    Subject = subject,
                    Body = htmlMessage,
                    IsBodyHtml = true
                };

                await smtpClient.SendMailAsync(message);
            }
        }
    }
}
