namespace AnteyaSidOnContainers.Services.Catalog.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using AnteyaSidOnContainers.Services.Catalog.Data.Common.Models;

    public class CatalogItem : DeletableEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [Range(0.0, Double.MaxValue)]
        public decimal Price { get; set; }

        public string Color { get; set; }
    }
}
