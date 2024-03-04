namespace Projet.Services
{
    using System.Threading.Tasks;
    public interface IMailingService
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
