namespace AnteyaSidOnContainers.Services.Catalog.API.Infrastructure.AutofacModules
{
    using System.Reflection;

    using Autofac;

    using AnteyaSidOnContainers.BuildingBlocks.EventBus.EventBus.AnteyaSid.Abstractions;
    using AnteyaSidOnContainers.Services.Catalog.API.IntegrationEvents.Events;

    public class ApplicationModule
       : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(CatalogItemCreatedIntegrationEvent).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IIntegrationEventHandler<>));
        }
    }
}
