namespace Projet
{
    public interface IEmailSend
    {
        Task SendEmailAsync(string Email, string subject, string message);
    }
}