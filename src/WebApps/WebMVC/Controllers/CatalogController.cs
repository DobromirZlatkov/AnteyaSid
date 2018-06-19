namespace AnteyaSidOnContainers.WebApps.WebMVC.Controllers
{
    using AnteyaSidOnContainers.WebApps.WebMVC.Services.Contracts;
    using Microsoft.AspNetCore.Mvc;
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

        [HttpPost]
        public async Task<IActionResult> GetCatalogItems()
        {
            var catalog = await _catalogSvc.GetCatalogItemsJson("?page=1&pageSize=1");
            return this.Json(catalog);
        }
    }
}
