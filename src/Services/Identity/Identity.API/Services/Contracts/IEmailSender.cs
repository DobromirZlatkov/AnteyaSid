using System.Threading.Tasks;

namespace AnteyaSidOnContainers.Services.Identity.API.Services.Contracts
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
