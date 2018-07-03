namespace AnteyaSidOnContainers.Services.Catalog.Data.Contracts
{
    using Microsoft.EntityFrameworkCore;

    using AnteyaSidOnContainers.Services.Catalog.Data.Models;
    using AnteyaSidOnContainers.Services.Catalog.Data.Models.Idempotency;

    public interface ICatalogDbContext
    {
        DbSet<CatalogItem> CatalogItems { get; }

        DbSet<ClientRequest> ClientRequests { get; }

        DbContext DbContext { get; }

        DbSet<T> Set<T>() where T : class;

        int SaveChanges();

        void Dispose();
    }
}
