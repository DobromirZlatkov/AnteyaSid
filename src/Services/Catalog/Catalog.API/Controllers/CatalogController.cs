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
    using AnteyaSidOnContainers.Services.Catalog.API.ViewModels;

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


        // POST api/v1/Catalog/CreateItem/
        // Data : { Name : <name>, Price: <price>, Color: <color> }
        [Route("[action]")]
        [HttpPost]
        public async Task<ActionResult> CreateItem(CatalogItemCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _catalogItemService.CreateNew(model.Name, model.Price, model.Color);
                return this.Ok(model);
            }

            return this.BadRequest(ModelState);
        }
    }
}
