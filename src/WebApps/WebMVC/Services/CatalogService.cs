namespace AnteyaSidOnContainers.WebApps.WebMVC.Services
{
    using System.Threading.Tasks;
    
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    
    using AnteyaSidOnContainers.WebApps.WebMVC.Infrastructure;
    using AnteyaSidOnContainers.WebApps.WebMVC.Services.Contracts;
    using AnteyaSidOnContainers.BuildingBlocks.Resilience.Http.Contracts;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Authentication;

    public class CatalogService : ICatalogService, IRemoteCrudService
    {
        private readonly IOptionsSnapshot<AppSettings> _settings;
        private readonly IHttpClient _apiClient;
        private readonly ILogger<CatalogService> _logger;
        private readonly IHttpContextAccessor _httpContextAccesor;

        private readonly string _remoteServiceBaseUrl;

        public CatalogService(
            IOptionsSnapshot<AppSettings> settings,
            IHttpClient httpClient,
            IHttpContextAccessor httpContextAccesor,
            ILogger<CatalogService> logger)
        {
            _settings = settings;
            _apiClient = httpClient;
            _httpContextAccesor = httpContextAccesor;
            _logger = logger;

            _remoteServiceBaseUrl = $"{_settings.Value.CatalogUrl}/api/v1/catalog/";
        }

        public async Task<string> GetData(string queryParams)
        {
            var token = await GetUserTokenAsync();
            var allcatalogItemsUri = API.Catalog.GetAllCatalogItems(_remoteServiceBaseUrl, queryParams);
            var dataString = await _apiClient.GetStringAsync(allcatalogItemsUri, token);

            return dataString;
        }


        public async Task<string> Create(object jsonObject)
        {
            var url = API.Catalog.CreateCatalogItem(_remoteServiceBaseUrl);

            var response = await _apiClient.PostAsync(url, jsonObject);

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> Delete(int id)
        {
            var url = API.Catalog.DeleteCatalogItem(_remoteServiceBaseUrl, id);

            var response = await _apiClient.PostAsync(url, id);

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> Update(object jsonObject)
        {
            var url = API.Catalog.UpdateCatalogItem(_remoteServiceBaseUrl);

            var response = await _apiClient.PostAsync(url, jsonObject);

            return await response.Content.ReadAsStringAsync();
        }

        async Task<string> GetUserTokenAsync()
        {
            var context = _httpContextAccesor.HttpContext;
            return await context.GetTokenAsync("access_token");
        }
    }
}
