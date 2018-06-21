namespace AnteyaSidOnContainers.Services.Catalog.API.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class CatalogItem
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [Range(0.0, Double.MaxValue)]
        public decimal Price { get; set; }

        public string Color { get; set; }
    }
}
