namespace AnteyaSidOnContainers.Services.Catalog.API
{
    public class CatalogSettings
    {
        public string PicBaseUrl { get; set; }

        public string EventBusConnection { get; set; }

        // TODO delete these two props not used
        public bool UseCustomizationData { get; set; }

        public bool AzureStorageEnabled { get; set; }
    }
}
