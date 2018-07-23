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
        
        /// <summary>
        /// POST or GET api/v1/[controller]/items[?PageSize=3&Page=10]
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("[action]")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Items(DataSourceRequest request)
        {
            var itemsQuery = _catalogItemService.GetAll();
            return Ok(itemsQuery.ToDataSourceResult(request, ModelState));
        }

        /// <summary>
        /// POST api/v1/Catalog/CreateItem/
        /// Data : { Name : <name>, Price: <price>, Color: <color> }
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> CreateItem([FromBody]CatalogItemCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _catalogItemService.CreateNew(model.Name, model.Price, model.Color);
                return Ok(model);
            }

            return BadRequest(ModelState);
        }

        /// <summary>
        /// POST api/v1/Catalog/UpdateItem/
        /// Data : { Id: <Catalog Item ID>, Name : <name>, Price: <price>, Color: <color> }
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> UpdateItem([FromBody]CatalogItemUpdateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var updatedModel = await _catalogItemService.Update(model.Id, model.Name, model.Price, model.Color);
                return Ok(model);
            }

            return BadRequest(ModelState);
        }

        /// <summary>
        /// POST api/v1/Catalog/DeleteItem/<ItemId>
        /// 
        /// Deletes catalog item from the database
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        [Route("[action]/[id]")]
        [HttpPost]
        public async Task<IActionResult> DeleteItem(int id)
        {
            if (!await _catalogItemService.doExistsById(id))
            {
                return NotFound($"Catalog items with id: {id} does not exists");
            }

            var catalogItemToBeDeleted = await _catalogItemService.Delete(id);

            return Ok();
        }
    }
}
