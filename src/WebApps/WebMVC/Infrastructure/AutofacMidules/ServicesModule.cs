namespace AnteyaSidOnContainers.WebApps.WebMVC.Infrastructure.AutofacMidules
{
    using AnteyaSidOnContainers.WebApps.WebMVC.Services;
    using AnteyaSidOnContainers.WebApps.WebMVC.Services.Contracts;
    using Autofac;
    using System.Reflection;

    public class ServicesModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(CatalogService).GetTypeInfo().Assembly)
                .AssignableTo<IService>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}