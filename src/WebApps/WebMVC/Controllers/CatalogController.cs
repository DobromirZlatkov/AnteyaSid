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

            var test = Request.QueryString.ToUriComponent();
            var catalogResponse = await _catalogSvc.GetCatalogItemsJson(Request.QueryString.ToUriComponent());

            //var catalogAsDatasourceResult = JsonConvert.DeserializeObject<CatalogItemDataSourceResult>(catalogResponse);

            return this.Ok(catalogResponse);
        }
    }
}
