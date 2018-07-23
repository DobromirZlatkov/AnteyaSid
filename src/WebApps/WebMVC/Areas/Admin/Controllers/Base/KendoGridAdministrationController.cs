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

    public abstract class KendoGridAdministrationController : AdminController
    {
        protected readonly IRemoteCrudService _remoteCrudService;

        public KendoGridAdministrationController(IRemoteCrudService remoteCrudService)
           : base()
        {
            _remoteCrudService = remoteCrudService ?? throw new ArgumentNullException(nameof(remoteCrudService));
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
        protected virtual async Task<T> Update<T>(object model) where T : class
        {
            if (model != null && ModelState.IsValid)
            {
                var jsonResponse = await this._remoteCrudService.Update(model);
                return JsonConvert.DeserializeObject<T>(jsonResponse);
            }

            return null;
        }

        protected JsonResult GridOperation<T>(T model, DataSourceRequest request)
        {
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }
    }
}
