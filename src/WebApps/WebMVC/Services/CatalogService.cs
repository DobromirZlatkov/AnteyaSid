namespace AnteyaSidOnContainers.WebApps.WebMVC.Services
{
    using System.Threading.Tasks;
    
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    
    using AnteyaSidOnContainers.WebApps.WebMVC.Infrastructure;
    using AnteyaSidOnContainers.WebApps.WebMVC.Services.Contracts;
    using AnteyaSidOnContainers.BuildingBlocks.Resilience.Http.Contracts;

    public class CatalogService : ICatalogService, IRemoteCrudService
    {
        private readonly IOptionsSnapshot<AppSettings> _settings;
        private readonly IHttpClient _apiClient;
        private readonly ILogger<CatalogService> _logger;
        
        private readonly string _remoteServiceBaseUrl;

        public CatalogService(
            IOptionsSnapshot<AppSettings> settings,
            IHttpClient httpClient,
            ILogger<CatalogService> logger)
        {
            _settings = settings;
            _apiClient = httpClient;
            _logger = logger;

            _remoteServiceBaseUrl = $"{_settings.Value.CatalogUrl}/api/v1/catalog/";
        }

        public async Task<string> GetData(string queryParams)
        {
            var allcatalogItemsUri = API.Catalog.GetAllCatalogItems(_remoteServiceBaseUrl, queryParams);

            var dataString = await _apiClient.GetStringAsync(allcatalogItemsUri);

            return dataString;
        }


        public async Task<string> Create(object jsonObject)
        {
            var url = API.Catalog.CreateCatalogItem(_remoteServiceBaseUrl);

            var response = await _apiClient.PostAsync(url, jsonObject);

            return await response.Content.ReadAsStringAsync();
        }

        public Task<string> Delete(object id)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> Update(object id, string jsonObject)
        {
            throw new System.NotImplementedException();
        }
    }
}
