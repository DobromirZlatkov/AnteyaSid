using System.Threading.Tasks;

namespace AnteyaSidOnContainers.Services.Identity.API.Services.Contracts
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
