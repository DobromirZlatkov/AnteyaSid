namespace AnteyaSidOnContainers.Services.Catalog.API
{
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Logging;

    using AnteyaSidOnContainers.BuildingBlocks.WebHost.Customization;
    using AnteyaSidOnContainers.Services.Catalog.API.Data;
    using AnteyaSidOnContainers.BuildingBlocks.EventBus.IntegrationEventLogEF;
    using System.IO;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;

    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args)
              .MigrateDbContext<CatalogContext>((_, __) => { })
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
