namespace AnteyaSidOnContainers.WebApps.WebMVC.Services.Contracts
{
    using System.Threading.Tasks;

    public interface IAuthService : IService
    {
        Task<string> GetUserTokenAsync();
    }
}
