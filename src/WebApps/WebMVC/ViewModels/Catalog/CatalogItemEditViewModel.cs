namespace AnteyaSidOnContainers.WebApps.WebMVC.ViewModels.Catalog
{
    using System.ComponentModel.DataAnnotations;

    public class CatalogItemEditViewModel
    {
        public string Name { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        public string Color { get; set; }
    }
}
