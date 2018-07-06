namespace AnteyaSidOnContainers.Services.Catalog.API.Application.IntegrationEvents.Events
{
    using System;

    using AnteyaSidOnContainers.BuildingBlocks.EventBus.EventBus.AnteyaSid.Events;

    public class CatalogItemUpdateIntegrationEvent : IntegrationEvent
    {
        public Guid RequestId { get; set; }

        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Color { get; set; }

        public CatalogItemUpdateIntegrationEvent(int id, string name, decimal price, string color, Guid requestId)
        {
            this.Id = id;
            this.RequestId = requestId;
            this.Name = name;
            this.Price = price;
            this.Color = color;
        }
    }
}
