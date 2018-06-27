namespace AnteyaSidOnContainers.WebApps.WebMVC.ViewModels.Catalog
{
    using System.ComponentModel.DataAnnotations;

    using AnteyaSidOnContainers.WebApps.WebMVC.ViewModels.Base;

    public class CatalogItemEditViewModel : BaseIntegrationEventViewModel
    {
        public string Name { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        public string Color { get; set; }
    }
}
