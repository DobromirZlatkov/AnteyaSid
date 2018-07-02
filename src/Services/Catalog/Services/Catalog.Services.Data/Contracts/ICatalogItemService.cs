namespace AnteyaSidOnContainers.Services.Catalog.Services.Data.Contracts
{
    using System.Linq;
    using System.Threading.Tasks;

    using AnteyaSidOnContainers.Services.Catalog.Data.Models;

    public interface ICatalogItemService : IService
    {
        IQueryable<CatalogItem> GetAll();

        Task<int> CreateNew(string name, decimal price, string color);
    }
}
