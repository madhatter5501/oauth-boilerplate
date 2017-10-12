using System.Threading.Tasks;

namespace Boilerplate.OAuth.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
