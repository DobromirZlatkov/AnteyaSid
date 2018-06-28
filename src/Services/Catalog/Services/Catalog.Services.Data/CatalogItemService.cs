namespace AnteyaSidOnContainers.Services.Catalog.Services.Data
{
    using System.Linq;

    using AnteyaSidOnContainers.Services.Catalog.Data.Contracts;
    using AnteyaSidOnContainers.Services.Catalog.Data.Models;
    using AnteyaSidOnContainers.Services.Catalog.Services.Data.Contracts;

    public class CatalogItemService : ICatalogItemService
    {
       // private readonly IDeletableEntityRepository<CatalogItem> _catalogItemsRepository;

        public CatalogItemService(
           // IDeletableEntityRepository<CatalogItem> catalogItemsRepository
            )
        {
         //   this._catalogItemsRepository = catalogItemsRepository;
        }

        public IQueryable<CatalogItem> GetAll()
        {
            throw new System.Exception("test");
           // return _catalogItemsRepository.All();
        }
    }
}
