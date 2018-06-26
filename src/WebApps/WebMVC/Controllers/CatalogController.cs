namespace AnteyaSidOnContainers.WebApps.WebMVC.Controllers
{
    using System.Threading.Tasks;
    
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Primitives;

    using Kendo.Mvc.UI;
    
    using AnteyaSidOnContainers.WebApps.WebMVC.Services.Contracts;
    
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
            Request.Form.TryGetValue("page", out StringValues page);
            Request.Form.TryGetValue("pageSize", out StringValues pageSize);
            Request.Form.TryGetValue("sort", out StringValues sort);
            Request.Form.TryGetValue("group", out StringValues group);
            Request.Form.TryGetValue("filter", out StringValues filter);

            var catalogResponse = await _catalogSvc.GetCatalogItemsJson($"?page={page}&pageSize={pageSize}&sort={sort}&group={group}&filter={filter}");

            return this.Ok(catalogResponse);
        }
    }
}
