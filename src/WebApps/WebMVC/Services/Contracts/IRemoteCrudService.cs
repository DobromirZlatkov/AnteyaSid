namespace AnteyaSidOnContainers.WebApps.WebMVC.Services.Contracts
{
    using System.Threading.Tasks;

    public interface IRemoteCrudService
    {
        Task<string> GetData(string queryParams);

        Task<string> Create(object jsonObject);

        Task<string> Update(object jsonObject);

        Task<string> Delete(int id);
    }
}
