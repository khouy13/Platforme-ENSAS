namespace Projet.Services
{
    // EmailSender.cs

    using System.Net;
    using System.Net.Mail;
    using System.Threading.Tasks;

    public class MailingService : IMailingService
    {
        private readonly string smtpServer;
        private readonly int smtpPort;
        private readonly string smtpUsername;
        private readonly string smtpPassword;
        private readonly string fromEmail;

       
        public MailingService(IConfiguration configuration)
        {
            // Récupérez les informations SMTP à partir de la configuration
            var emailConfiguration = configuration.GetSection("EmailConfiguration");

            smtpServer = emailConfiguration["SmtpServer"];
            smtpPort = int.Parse(emailConfiguration["SmtpPort"]);
            smtpUsername = emailConfiguration["SmtpUsername"];
            smtpPassword = emailConfiguration["SmtpPassword"];
            fromEmail = emailConfiguration["FromEmail"]; // Récupérez l'adresse e-mail de l'expéditeur
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            using (var client = new SmtpClient(smtpServer))
            {
                client.Port = smtpPort;
                client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                client.EnableSsl = true;

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(smtpUsername),
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(email);

                await client.SendMailAsync(mailMessage);
            }
        }
    }

}
