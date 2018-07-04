namespace AnteyaSidOnContainers.WebApps.WebMVC.Areas.Admin.Controllers.Base
{
    using AnteyaSidOnContainers.WebApps.WebMVC.Services.Contracts;
    using AutoMapper;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using System;
    using System.Collections;
    using System.Threading.Tasks;

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

        //[NonAction]
        //protected virtual void Update<TModel, TViewModel>(TViewModel model, object id)
        //    where TModel : class
        //    where TViewModel : class
        //{
        //    if (model != null && ModelState.IsValid)
        //    {
        //        var dbModel = this.GetById<TModel>(id);
        //        Mapper.Map<TViewModel, TModel>(model, dbModel);
        //        this.ChangeEntityStateAndSave(dbModel, EntityState.Modified);
        //    }
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
