namespace AnteyaSidOnContainers.Services.Catalog.Services.Data.Contracts
{
    using System.Linq;

    using AnteyaSidOnContainers.Services.Catalog.Data.Models;

    public interface ICatalogItemService : IService
    {
        IQueryable<CatalogItem> GetAll();
    }
}
