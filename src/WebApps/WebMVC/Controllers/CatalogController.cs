namespace AnteyaSidOnContainers.WebApps.WebMVC.Controllers
{
    using AnteyaSidOnContainers.WebApps.WebMVC.Extensions;
    using AnteyaSidOnContainers.WebApps.WebMVC.Services.Contracts;
    using AnteyaSidOnContainers.WebApps.WebMVC.ViewModels.Catalog;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Primitives;
    using Newtonsoft.Json;
    using System.Text;
    using System.Threading.Tasks;

    public class CatalogController: Controller
    {
        private ICatalogService _catalogSvc;

        public CatalogController(ICatalogService catalogSvc)
        {
            _catalogSvc = catalogSvc;
        }

        public IActionResult Index()
        {
            return View();
        }
        
        public async Task<IActionResult> GetCatalogItems([DataSourceRequest]DataSourceRequest request)
        {
            Request.Form.TryGetValue("page", out StringValues page);
            Request.Form.TryGetValue("pageSize", out StringValues pageSize);
            Request.Form.TryGetValue("sort", out StringValues sort);
            Request.Form.TryGetValue("group", out StringValues group);
            Request.Form.TryGetValue("filter", out StringValues filter);

            var catalogResponse = await _catalogSvc.GetCatalogItemsJson($"?page={page}&pageSize={pageSize}&sort={sort}&group={group}&filter={filter}");

            var catalogAsDatasourceResult = JsonConvert.DeserializeObject<CatalogItemDataSourceResult>(catalogResponse);

            return this.Json(catalogAsDatasourceResult.data.ToDataSourceResult(request));
        }
    }
}
