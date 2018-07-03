namespace AnteyaSidOnContainers.Services.Catalog.API.Infrastructure.AutofacModules
{
    using System.Reflection;

    using Autofac;

    using AnteyaSidOnContainers.BuildingBlocks.EventBus.EventBus.AnteyaSid.Abstractions;
    using AnteyaSidOnContainers.Services.Catalog.API.Application.Commands;

    public class ApplicationModule
       : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(CreateCatalogItemCommand).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IIntegrationEventHandler<>));
        }
    }
}
