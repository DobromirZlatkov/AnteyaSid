namespace AnteyaSidOnContainers.Services.Catalog.API.Application.IntegrationEvents.Events
{
    using System;

    using AnteyaSidOnContainers.BuildingBlocks.EventBus.EventBus.AnteyaSid.Events;

    public class CatalogItemDeleteIntegrationEvent : IntegrationEvent
    {
        public Guid RequestId { get; set; }

        public int Id { get; set; }
    }
}
