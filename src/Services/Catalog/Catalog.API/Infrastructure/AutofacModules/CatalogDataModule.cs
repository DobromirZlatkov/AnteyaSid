namespace AnteyaSidOnContainers.Services.Catalog.API.Infrastructure.AutofacModules
{
    using Autofac;

    using AnteyaSidOnContainers.Services.Catalog.Data;
    using AnteyaSidOnContainers.Services.Catalog.Data.Contracts;

    public class CatalogDataModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                 .RegisterGeneric(typeof(DeletableEntityRepository<>))
                 .As(typeof(IDeletableEntityRepository<>));

            builder
                .RegisterGeneric(typeof(EfGenericRepository<>))
                .As(typeof(IRepository<>));

            builder
                .RegisterType<CatalogDbContext>()
                .As<ICatalogDbContext>()
                .InstancePerRequest();
        }
    }
}
