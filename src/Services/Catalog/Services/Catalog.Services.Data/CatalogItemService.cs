namespace AnteyaSidOnContainers.Services.Catalog.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using AnteyaSidOnContainers.Services.Catalog.Data.Contracts;
    using AnteyaSidOnContainers.Services.Catalog.Data.Models;
    using AnteyaSidOnContainers.Services.Catalog.Services.Data.Contracts;

    public class CatalogItemService : ICatalogItemService
    {
        private readonly IDeletableEntityRepository<CatalogItem> _catalogItemsRepository;

        public CatalogItemService(
            IDeletableEntityRepository<CatalogItem> catalogItemsRepository
        )
        {
            this._catalogItemsRepository = catalogItemsRepository ?? throw new ArgumentNullException(nameof(catalogItemsRepository));
        }

        public async Task<int> CreateNew(string name, decimal price, string color)
        {
            var catalogItem = new CatalogItem()
            {
                Name = name,
                Price = price,
                Color = color,
            };

            _catalogItemsRepository.Add(catalogItem);

            return await _catalogItemsRepository.SaveChangesAsync();
        }

        public async Task<int> Delete(int id)
        {
            _catalogItemsRepository.Delete(id);

            return await this._catalogItemsRepository.SaveChangesAsync();
        }

        public IQueryable<CatalogItem> GetAll()
        {
            return _catalogItemsRepository.All();
        }

        public async Task<CatalogItem> Update(int id, string name, decimal price, string color)
        {
            var catalogItem = this._catalogItemsRepository.GetById(id);

            catalogItem.Name = name;
            catalogItem.Price = price;
            catalogItem.Color = color;

            await _catalogItemsRepository.SaveChangesAsync();

            return catalogItem;
        }
    }
}
