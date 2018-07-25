using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using AnteyaSidOnContainers.Services.Identity.API.Configuration;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace AnteyaSidOnContainers.Services.Identity.API.Data
{
    public class ConfigurationDbContextSeed
    {
        public async Task SeedAsync(ConfigurationDbContext context, IConfiguration configuration)
        {
            //callbacks urls from config:
            var clientUrls = new Dictionary<string, string>();

            clientUrls.Add("Mvc", configuration.GetValue<string>("MvcClient"));
            clientUrls.Add("CatalogApi", configuration.GetValue<string>("CatalogUrl"));

            /// Delete all clients and repopulated, to make dev process easier
            var clientsToDelete = context.Clients;

            foreach (var client in clientsToDelete)
            {
                context.Remove(client);
                await context.SaveChangesAsync();
            }

            /// Delete all IdentityResources and repopulated, to make dev process easier
            var identityResourcesToDelete = context.IdentityResources;

            foreach (var identityResource in identityResourcesToDelete)
            {
                context.Remove(identityResource);
                await context.SaveChangesAsync();
            }

            /// Delete all ApiResources and repopulated, to make dev process easier
            var apiResourcesToDelete = context.ApiResources;

            foreach (var apiResource in apiResourcesToDelete)
            {
                context.Remove(apiResource);
                await context.SaveChangesAsync();
            }

            /// Populate all resources and clients
            if (!context.Clients.Any())
            {
                foreach (var client in Config.GetClients(clientUrls))
                {
                    await context.Clients.AddAsync(client.ToEntity());
                }
                await context.SaveChangesAsync();
            }

            if (!context.IdentityResources.Any())
            {
                foreach (var resource in Config.GetResources())
                {
                    await context.IdentityResources.AddAsync(resource.ToEntity());
                }
                await context.SaveChangesAsync();
            }

            if (!context.ApiResources.Any())
            {
                foreach (var api in Config.GetApis())
                {
                    await context.ApiResources.AddAsync(api.ToEntity());
                }

                await context.SaveChangesAsync();
            }
        }
    }
}
