namespace AnteyaSidOnContainers.WebApps.WebMVC.Areas.Admin.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Kendo.Mvc.UI;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Primitives;

    using AnteyaSidOnContainers.WebApps.WebMVC.Areas.Admin.Controllers.Base;
    using AnteyaSidOnContainers.WebApps.WebMVC.Services.Contracts;
    using AnteyaSidOnContainers.WebApps.WebMVC.ViewModels.Catalog;
    using AnteyaSidOnContainers.BuildingBlocks.EventBus.EventBus.AnteyaSid.Abstractions;
    using AnteyaSidOnContainers.WebApps.WebMVC.IntegrationEvents.Events.Catalog;

    //[Route("admin/catalog")]
    public class CatalogAdminController : KendoGridAdministrationController
    {
        public CatalogAdminController(
            ICatalogService catalogService,
            IEventBus eventBus
        ) : base(catalogService, eventBus)
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        protected override T GetById<T>(object id)
        {
            throw new NotImplementedException();
        }

        protected override async Task<string> GetData(DataSourceRequest request)
        {
            Request.Form.TryGetValue("page", out StringValues page);
            Request.Form.TryGetValue("pageSize", out StringValues pageSize);
            Request.Form.TryGetValue("sort", out StringValues sort);
            Request.Form.TryGetValue("group", out StringValues group);
            Request.Form.TryGetValue("filter", out StringValues filter);

            return await this._remoteCrudService.GetData($"?page={page}&pageSize={pageSize}&sort={sort}&group={group}&filter={filter}");
        }

        [HttpPost]
        public async Task<IActionResult> Create(DataSourceRequest request, CatalogItemCreateViewModel model, [FromHeader(Name = "x-requestid")] string requestId)
        {
            var dbModel = await this.Create<CatalogItemCreateViewModel>(model);
            return this.GridOperation(model, request);
        }

        [HttpPost]
        public IActionResult Update(DataSourceRequest request, CatalogItemEditViewModel model, [FromHeader(Name = "x-requestid")] string requestId)
        {
            model.RequestId = (Guid.TryParse(requestId, out Guid guid) && guid != Guid.Empty) ?
                guid : Guid.NewGuid();

            this.SendEvent<CatalogItemUpdateIntegrationEvent>(model);

            return this.GridOperation(model, request);
        }

        [HttpPost]
        public ActionResult Destroy(DataSourceRequest request, CatalogItemEditViewModel model, [FromHeader(Name = "x-requestid")] string requestId)
        {
            model.RequestId = (Guid.TryParse(requestId, out Guid guid) && guid != Guid.Empty) ?
               guid : Guid.NewGuid();

            this.SendEvent<CatalogItemDeleteIntegrationEvent>(model);

            return this.GridOperation(model, request);
        }

        //[NonAction]
        //protected virtual T Create<T>(object model) where T : IntegrationEvent
        //{
        //    if (model != null && ModelState.IsValid)
        //    {
        //        var eventModel = Mapper.Map<T>(model);
        //        _eventBus.Publish(eventModel);

        //        return eventModel;
        //    }

        //    return null;
        //}

        //protected JsonResult GridOperation<T>(T model, DataSourceRequest request)
        //{
        //    return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        //}
    }
}
