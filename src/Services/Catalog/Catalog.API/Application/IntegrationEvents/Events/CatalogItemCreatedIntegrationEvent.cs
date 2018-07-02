namespace AnteyaSidOnContainers.Services.Catalog.API.Application.IntegrationEvents.Events
{
    using System;

    using AnteyaSidOnContainers.BuildingBlocks.EventBus.EventBus.AnteyaSid.Events;

    public class CatalogItemCreatedIntegrationEvent : IntegrationEvent
    {
        public Guid RequestId { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Color { get; set; }

        public CatalogItemCreatedIntegrationEvent(string name, decimal price, string color, Guid requestId)
        {
            this.RequestId = requestId;
            this.Name = name;
            this.Price = price;
            this.Color = color;
        }
    }
}
