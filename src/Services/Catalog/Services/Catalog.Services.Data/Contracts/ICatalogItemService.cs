namespace AnteyaSidOnContainers.Services.Catalog.Services.Data.Contracts
{
    using System.Linq;
    using System.Threading.Tasks;

    using AnteyaSidOnContainers.Services.Catalog.Data.Models;

    public interface ICatalogItemService : IService
    {
        IQueryable<CatalogItem> GetAll();

        Task<CatalogItem> CreateNew(string name, decimal price, string color);

        Task<CatalogItem> Update(int id, string name, decimal price, string color);

        Task<int> Delete(int id);

        Task<bool> doExistsById(int itemId);
    }
}
