namespace AnteyaSidOnContainers.WebApps.WebMVC.IntegrationEvents.Events.Catalog
{
    using System;

    using AnteyaSidOnContainers.BuildingBlocks.EventBus.EventBus.AnteyaSid.Events;
    using AnteyaSidOnContainers.WebApps.WebMVC.Infrastructure.Mapping;
    using AnteyaSidOnContainers.WebApps.WebMVC.ViewModels.Catalog;

    public class CatalogItemDeleteIntegrationEvent : IntegrationEvent, IMapFrom<CatalogItemDeleteViewModel>
    {
        public Guid RequestId { get; set; }

        public int Id { get; set; }
    }
}
