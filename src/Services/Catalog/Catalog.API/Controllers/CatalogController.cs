namespace AnteyaSidOnContainers.Services.Catalog.API.Controllers
{
    using System;
    using System.Linq;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;

    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;

    using AnteyaSidOnContainers.Services.Catalog.API.Data;

    [Route("api/v1/[controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly CatalogContext _catalogContext;
        private readonly CatalogSettings _settings;

        public CatalogController(
            CatalogContext context, 
            IOptionsSnapshot<CatalogSettings> settings)
        {
            _catalogContext = context ?? throw new ArgumentNullException(nameof(context));

            _settings = settings.Value;

            ((DbContext)context).ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }
        
        // POST api/v1/[controller]/items[?PageSize=3&Page=10]
        // TODO document api, Add service layer, data layer, repository pattern, etc. For now until deploy and consumed by main MVC app can stay like this
        [Route("[action]")]
        public ActionResult Items(DataSourceRequest request)
        {
            var itemsQuery = _catalogContext.CatalogItems.AsQueryable();

            var response = itemsQuery.ToDataSourceResult(request, this.ModelState);

            return Ok(response);
        }
    }
}
