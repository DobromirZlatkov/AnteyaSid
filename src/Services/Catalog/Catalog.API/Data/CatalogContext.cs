namespace AnteyaSidOnContainers.Services.Catalog.API.Data
{
    using Microsoft.EntityFrameworkCore;
    //using EntityConfigurations;
    //using Model;
    using Microsoft.EntityFrameworkCore.Design;

    public class CatalogContext : DbContext
    {
        public CatalogContext(DbContextOptions<CatalogContext> options) : base(options)
        {
        }
        //public DbSet<CatalogItem> CatalogItems { get; set; }
        //public DbSet<CatalogBrand> CatalogBrands { get; set; }
        //public DbSet<CatalogType> CatalogTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //builder.ApplyConfiguration(new CatalogBrandEntityTypeConfiguration());
            //builder.ApplyConfiguration(new CatalogTypeEntityTypeConfiguration());
            //builder.ApplyConfiguration(new CatalogItemEntityTypeConfiguration());
        }
    }


    public class CatalogContextDesignFactory : IDesignTimeDbContextFactory<CatalogContext>
    {
        public CatalogContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CatalogContext>()
                .UseNpgsql("User ID = postgres;Password=56;Server=192.168.1.118;Port=5432;Database=Catalog.db;Integrated Security=true; Pooling=true");

            return new CatalogContext(optionsBuilder.Options);
        }
    }
}
