namespace AnteyaSidOnContainers.Services.Catalog.API
{
    using System.IO;

    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    using AnteyaSidOnContainers.BuildingBlocks.EventBus.IntegrationEventLogEF;
    using AnteyaSidOnContainers.BuildingBlocks.WebHost.Customization;
    using AnteyaSidOnContainers.Services.Catalog.Data;

    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args)
              .MigrateDbContext<CatalogDbContext>((_, __) => { })
              .MigrateDbContext<IntegrationEventLogContext>((_, __) => { })
              .Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseApplicationInsights()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration((builderContext, config) =>
                {
                    config.AddEnvironmentVariables();
                })
                .ConfigureLogging((hostingContext, builder) =>
                {
                    builder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    builder.AddConsole();
                    builder.AddDebug();
                })
                .Build();
    }
}
