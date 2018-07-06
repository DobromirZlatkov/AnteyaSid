namespace AnteyaSidOnContainers.WebApps.WebMVC.Areas.Admin.Controllers.Base
{
    using System;
    using System.Threading.Tasks;

    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;

    using AnteyaSidOnContainers.BuildingBlocks.EventBus.EventBus.AnteyaSid.Abstractions;
    using AnteyaSidOnContainers.WebApps.WebMVC.Services.Contracts;
    using AutoMapper;
    using AnteyaSidOnContainers.BuildingBlocks.EventBus.EventBus.AnteyaSid.Events;

    public abstract class KendoGridAdministrationController : AdminController
    {
        protected readonly IRemoteCrudService _remoteCrudService;
        protected readonly IEventBus _eventBus;

        public KendoGridAdministrationController(IRemoteCrudService remoteCrudService, IEventBus eventBus)
           : base()
        {
            _remoteCrudService = remoteCrudService ?? throw new ArgumentNullException(nameof(remoteCrudService));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }

        protected abstract Task<string> GetData(DataSourceRequest request);

        protected abstract T GetById<T>(object id) where T : class;
        
        [HttpPost]
        public async Task<IActionResult> Read(DataSourceRequest request)
        {
            return this.Ok(await this.GetData(request));
        }

        [NonAction]
        protected virtual async Task<T> Create<T>(object model) where T : class
        {
            if (model != null && ModelState.IsValid)
            {
                var jsonResponse = await this._remoteCrudService.Create(model);
                return JsonConvert.DeserializeObject<T>(jsonResponse);
            }

            return null;
        }

        [NonAction]
        protected virtual void Update<T>(object model) where T : IntegrationEvent
        {
            if (model != null && ModelState.IsValid)
            {
                var eventModel = Mapper.Map<T>(model);
                _eventBus.Publish(eventModel);
            }
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


        protected JsonResult GridOperation<T>(T model, DataSourceRequest request)
        {
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        //private void ChangeEntityStateAndSave(object dbModel, EntityState state)
        //{
        //    var entry = this.Data.Context.Entry(dbModel);
        //    entry.State = state;
        //    this.Data.SaveChanges();
        //}
    }
}
