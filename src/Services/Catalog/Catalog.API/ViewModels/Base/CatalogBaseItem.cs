namespace AnteyaSidOnContainers.Services.Catalog.API.ViewModels.Base
{
    using System;

    public abstract class CatalogBaseItem
    {
        public Guid RequestId { get; set; }

        public string Name { get; set; }

        public string Color { get; set; }

        public decimal Price { get; set; }
    }
}
