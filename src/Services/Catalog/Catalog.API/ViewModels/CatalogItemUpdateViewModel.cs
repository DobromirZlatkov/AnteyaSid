namespace AnteyaSidOnContainers.Services.Catalog.API.ViewModels
{
    using AnteyaSidOnContainers.Services.Catalog.API.ViewModels.Base;
    using System.ComponentModel.DataAnnotations;

    public class CatalogItemUpdateViewModel : CatalogBaseItem
    {
        [Required]
        public int Id { get; set; }
    }
}
