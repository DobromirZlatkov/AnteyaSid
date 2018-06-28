namespace AnteyaSidOnContainers.Services.Catalog.API.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Linq;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;

    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;

    using AnteyaSidOnContainers.Services.Catalog.Data;
    using AnteyaSidOnContainers.Services.Catalog.Services.Data.Contracts;


    [Route("api/v1/[controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly ICatalogItemService _catalogItemService;

        public CatalogController(ICatalogItemService catalogItemService)
        {
            _catalogItemService = catalogItemService ?? throw new ArgumentNullException(nameof(catalogItemService));
        }
        
        // POST api/v1/[controller]/items[?PageSize=3&Page=10]
        [Route("[action]")]
        public ActionResult Items(DataSourceRequest request)
        {
            var itemsQuery = _catalogItemService.GetAll();
            return Ok(itemsQuery.ToDataSourceResult(request, this.ModelState));
        }
    }
}
