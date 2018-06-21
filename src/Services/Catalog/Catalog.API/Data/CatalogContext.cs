namespace AnteyaSidOnContainers.Services.Catalog.API.Data
{
    using AnteyaSidOnContainers.Services.Catalog.API.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;

    public class CatalogContext : DbContext
    {
        public CatalogContext(DbContextOptions<CatalogContext> options) : base(options)
        {
        }

        public DbSet<CatalogItem> CatalogItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //builder.ApplyConfiguration(new CatalogItemEntityTypeConfiguration());
        }
    }
}
