﻿namespace AnteyaSidOnContainers.WebApps.WebMVC.IntegrationEvents.Events.Catalog
{
    using System;

    using AnteyaSidOnContainers.BuildingBlocks.EventBus.EventBus.AnteyaSid.Events;
    using AnteyaSidOnContainers.WebApps.WebMVC.Infrastructure.Mapping;
    using AnteyaSidOnContainers.WebApps.WebMVC.ViewModels.Catalog;

    public class CatalogItemCreatedIntegrationEvent : IntegrationEvent, IMapFrom<CatalogItemEditViewModel>
    {
        public Guid RequestId { get; set; }

        public string Name { get; set; }
        
        public decimal Price { get; set; }

        public string Color { get; set; }
    }
}