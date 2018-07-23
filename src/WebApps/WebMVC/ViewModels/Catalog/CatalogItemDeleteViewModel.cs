namespace AnteyaSidOnContainers.WebApps.WebMVC.ViewModels.Catalog
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class CatalogItemDeleteViewModel
    {
        [Required]
        public int Id { get; set; }
    }
}
