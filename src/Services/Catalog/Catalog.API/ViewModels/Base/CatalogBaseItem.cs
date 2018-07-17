namespace AnteyaSidOnContainers.Services.Catalog.API.ViewModels.Base
{
    public abstract class CatalogBaseItem
    {
        public string Name { get; set; }

        public string Color { get; set; }

        public decimal Price { get; set; }
    }
}
