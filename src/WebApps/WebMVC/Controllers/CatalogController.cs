namespace AnteyaSidOnContainers.WebApps.WebMVC.Controllers
{
    using AnteyaSidOnContainers.WebApps.WebMVC.Services.Contracts;
    using AnteyaSidOnContainers.WebApps.WebMVC.ViewModels.Catalog;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
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
        
        public async Task<IActionResult> GetCatalogItems(DataSourceRequest request)
        {
            var catalogResponse = await _catalogSvc.GetCatalogItemsJson(string.Format("?page={0}&pageSize={1}", request.Page, request.PageSize));

            var catalogAsDatasourceResult = JsonConvert.DeserializeObject<CatalogItemDataSourceResult>(catalogResponse);

            return this.Json(catalogAsDatasourceResult.data.ToDataSourceResult(request, this.ModelState));
        }
    }
}
