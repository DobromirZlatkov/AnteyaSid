namespace AnteyaSidOnContainers.WebApps.WebMVC.Areas.Admin.Controllers
{
    using System;
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
    using AnteyaSidOnContainers.WebApps.WebMVC.Areas.Admin.Controllers.Base;
    using System.Collections;

    //[Route("admin/catalog")]
    public class CatalogAdminController : KendoGridAdministrationController
    {
        public CatalogAdminController(
            ICatalogService catalogService
        ) : base(catalogService)
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
        public async Task<IActionResult> Create(DataSourceRequest request, ViewModel model, [FromHeader(Name = "x-requestid")] string requestId)
        {
            var dbModel = await this.Create<CatalogItemCreatedIntegrationEvent>(model);
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
