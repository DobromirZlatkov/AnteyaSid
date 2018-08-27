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
    
    using AnteyaSidOnContainers.Services.Catalog.Services.Data.Contracts;
    using AnteyaSidOnContainers.Services.Catalog.API.ViewModels;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using AnteyaSidOnContainers.Services.Catalog.API.Application.Commands;
    using AnteyaSidOnContainers.Services.Catalog.Data.Models;
    using Microsoft.AspNetCore.Authorization;

    [Authorize]
    [Route("api/v1/[controller]/[action]/")]
    public class CatalogController : ControllerBase
    {
        private readonly ICatalogItemService _catalogItemService;
        private readonly IMediator _mediator;
        private readonly ILoggerFactory _logger;

        public CatalogController(
            ICatalogItemService catalogItemService,
            IMediator mediator,
            ILoggerFactory logger
            )
        {
            _catalogItemService = catalogItemService ?? throw new ArgumentNullException(nameof(catalogItemService));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// POST or GET api/v1/[controller]/items[?PageSize=3&Page=10]
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
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
        [HttpPost]
        public async Task<IActionResult> CreateItem([FromBody]CatalogItemCreateViewModel model, [FromHeader(Name = "x-requestid")] string requestId)
        {
            model.RequestId = (Guid.TryParse(requestId, out Guid guid) && guid != Guid.Empty) ?
                 guid : Guid.NewGuid();

            if (ModelState.IsValid)
            {
                if (model.RequestId != Guid.Empty)
                {
                    var createCatalogItemCommand = new CreateCatalogItemCommand(model.Name, model.Price, model.Color);

                    var requestUpdateCatalogItem = new IdentifiedCommand<CreateCatalogItemCommand, CatalogItem>(createCatalogItemCommand, model.RequestId);

                    var result = await _mediator.Send(requestUpdateCatalogItem);
                }

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
        /// POST api/v1/Catalog/DeleteItem/<ItemId>/
        /// 
        /// Deletes catalog item from the database
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns>
        /// 404 not found if Item is missing
        /// 202 accepted if item is deleted
        /// </returns>
        [HttpPost("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            if (!await _catalogItemService.doExistsById(id))
            {
                return NotFound($"Catalog items with id: {id} does not exists");
            }

            await _catalogItemService.Delete(id);

            return Accepted();
        }
    }
}
