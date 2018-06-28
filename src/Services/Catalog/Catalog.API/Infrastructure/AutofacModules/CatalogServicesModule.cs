namespace AnteyaSidOnContainers.Services.Catalog.API.Infrastructure.AutofacModules
{
    using System.Reflection;

    using Autofac;

    using AnteyaSidOnContainers.Services.Catalog.Services.Data.Contracts;
    using AnteyaSidOnContainers.Services.Catalog.Services.Data;

    public class CatalogServicesModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(CatalogItemService).GetTypeInfo().Assembly)
                .AssignableTo<IService>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}

