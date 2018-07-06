namespace AnteyaSidOnContainers.WebApps.WebMVC.ViewModels.Catalog
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class CatalogItemEditViewModel
    {
        [Required]
        public int Id { get; set; }

        public Guid RequestId { get; set; }

        public string Name { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        public string Color { get; set; }
    }
}
