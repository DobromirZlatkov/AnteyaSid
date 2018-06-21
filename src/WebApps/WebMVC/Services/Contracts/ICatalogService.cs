namespace AnteyaSidOnContainers.WebApps.WebMVC.Services.Contracts
{
    using System.Threading.Tasks;

    public interface ICatalogService
    {
        Task<string> GetCatalogItemsJson(string queryParams);
    }
}
