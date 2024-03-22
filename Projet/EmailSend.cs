
using Azure.Core;
using System.Configuration;
using System.Net;
using System.Net.Mail;
namespace Projet
{
    public class EmailSend : IEmailSend
    {
        public Task SendEmailAsync(string Email, string subject, string message)
        {
          
            var email = "abderrazzakkhouy790@gmail.com";
            var pass = "jugihtpxyqtnapdg";
            var client = new SmtpClient("smtp.gmail.com", 587) 
            
            {
               

                EnableSsl = true,
                Credentials = new NetworkCredential(email, pass)
            };

            MailMessage mm = new MailMessage(from: email, to: Email, subject, message);
            mm.Subject = subject;
            string body = message;
            mm.Body = body;
            mm.IsBodyHtml = true;
            
            return client.SendMailAsync(mm);
        }
    }
}
