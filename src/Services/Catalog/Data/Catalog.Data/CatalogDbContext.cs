﻿namespace AnteyaSidOnContainers.Services.Catalog.Data
{
    using Microsoft.EntityFrameworkCore;

    using AnteyaSidOnContainers.Services.Catalog.Data.Contracts;
    using AnteyaSidOnContainers.Services.Catalog.Data.Models;

    public class CatalogDbContext : DbContext, ICatalogDbContext
    {
        public DbSet<CatalogItem> CatalogItems { get; set; }

        public DbContext DbContext
        {
            get
            {
                return this;
            }
        }

        public CatalogDbContext(DbContextOptions<CatalogDbContext> options) : base(options)
        {
        }
    }
}
