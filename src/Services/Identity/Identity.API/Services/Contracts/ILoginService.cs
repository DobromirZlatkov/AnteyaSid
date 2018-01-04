using System.Threading.Tasks;

namespace AnteyaSidOnContainers.Services.Identity.API.Services.Contracts
{
    public interface ILoginService<T>
    {
        Task<bool> ValidateCredentials(T user, string password);
        Task<T> FindByUsername(string user);
        Task SignIn(T user);
    }
}
