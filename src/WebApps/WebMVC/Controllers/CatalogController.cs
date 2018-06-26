namespace AnteyaSidOnContainers.WebApps.WebMVC.Controllers
{
    using System.Threading.Tasks;
    
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Primitives;

    using Kendo.Mvc.UI;
    using Kendo.Mvc.Extensions;

    using AnteyaSidOnContainers.WebApps.WebMVC.Services.Contracts;

    using ViewModel = AnteyaSidOnContainers.WebApps.WebMVC.ViewModels.Catalog.CatalogItemEditViewModel;
    using AnteyaSidOnContainers.BuildingBlocks.EventBus.EventBus.AnteyaSid.Abstractions;
    using AnteyaSidOnContainers.WebApps.WebMVC.IntegrationEvents.Events.Catalog;
    using AutoMapper;
    using AnteyaSidOnContainers.BuildingBlocks.EventBus.EventBus.AnteyaSid.Events;

    public class CatalogController: Controller
    {
        private readonly ICatalogService _catalogService;
        private readonly IEventBus _eventBus;

        public CatalogController(
            IEventBus eventBus, 
            ICatalogService catalogSvc)
        {
            this._catalogService = catalogSvc;
            this._eventBus = eventBus;
        }

        public IActionResult Index()
        {
            return View();
        }
        
        public async Task<IActionResult> Read(DataSourceRequest request)
        {
            Request.Form.TryGetValue("page", out StringValues page);
            Request.Form.TryGetValue("pageSize", out StringValues pageSize);
            Request.Form.TryGetValue("sort", out StringValues sort);
            Request.Form.TryGetValue("group", out StringValues group);
            Request.Form.TryGetValue("filter", out StringValues filter);

            var catalogResponse = await this._catalogService.GetCatalogItemsJson($"?page={page}&pageSize={pageSize}&sort={sort}&group={group}&filter={filter}");

            return this.Ok(catalogResponse);
        }

        [HttpPost]
        public ActionResult Create(DataSourceRequest request, ViewModel model)
        {
            var dbModel = this.Create<CatalogItemCreatedIntegrationEvent>(model);
            //if (dbModel != null) model.Id = dbModel.Id;

            return this.GridOperation(model, request);
        }

        [NonAction]
        protected virtual T Create<T>(object model) where T : IntegrationEvent
        {
            if (model != null && ModelState.IsValid)
            {
                var eventModel = Mapper.Map<T>(model);
                _eventBus.Publish(eventModel);
                
                return eventModel;
            }

            return null;
        }

        protected JsonResult GridOperation<T>(T model, DataSourceRequest request)
        {
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }
    }
}
