using System;
using System.Collections.Generic;
namespace AnteyaSidOnContainers.Services.Catalog.Data.Contracts
{
    using Microsoft.EntityFrameworkCore;

    using AnteyaSidOnContainers.Services.Catalog.Data.Models;

    public interface ICatalogDbContext
    {
        DbSet<CatalogItem> CatalogItems { get; }

        DbContext DbContext { get; }

        DbSet<T> Set<T>() where T : class;

        int SaveChanges();

        void Dispose();
    }
}
