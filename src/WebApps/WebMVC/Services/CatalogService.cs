namespace AnteyaSidOnContainers.WebApps.WebMVC.Services
{
    using AnteyaSidOnContainers.BuildingBlocks.Resilience.Http.Contracts;
    using AnteyaSidOnContainers.WebApps.WebMVC.Infrastructure;
    using AnteyaSidOnContainers.WebApps.WebMVC.Services.Contracts;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using System;
    using System.Threading.Tasks;

    public class CatalogService : ICatalogService, IRemoteCrudService
    {
        private readonly IOptionsSnapshot<AppSettings> _settings;
        private readonly IHttpClient _apiClient;
        private readonly ILogger<CatalogService> _logger;
        private readonly IAuthService _authService;

        private readonly string _remoteServiceBaseUrl;

        public CatalogService(
            IOptionsSnapshot<AppSettings> settings,
            IHttpClient httpClient,
            IAuthService authService,
            ILogger<CatalogService> logger)
        {
            _settings = settings;
            _apiClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _remoteServiceBaseUrl = $"{_settings.Value.CatalogUrl}/api/v1/catalog/";
        }

        public async Task<string> GetData(string queryParams)
        {
            var token = await _authService.GetUserTokenAsync();
            var allcatalogItemsUri = API.Catalog.GetAllCatalogItems(_remoteServiceBaseUrl, queryParams);
            var dataString = await _apiClient.GetStringAsync(allcatalogItemsUri, token);

            return dataString;
        }

        public async Task<string> Create(object jsonObject)
        {
            var token = await _authService.GetUserTokenAsync();
            var url = API.Catalog.CreateCatalogItem(_remoteServiceBaseUrl);
            var response = await _apiClient.PostAsync(url, jsonObject, token);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> Delete(int id)
        {
            var token = await _authService.GetUserTokenAsync();
            var url = API.Catalog.DeleteCatalogItem(_remoteServiceBaseUrl, id);
            var response = await _apiClient.PostAsync(url, id, token);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> Update(object jsonObject)
        {
            var token = await _authService.GetUserTokenAsync();
            var url = API.Catalog.UpdateCatalogItem(_remoteServiceBaseUrl);
            var response = await _apiClient.PostAsync(url, jsonObject, token);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
