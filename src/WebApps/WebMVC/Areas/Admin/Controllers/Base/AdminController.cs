namespace AnteyaSidOnContainers.WebApps.WebMVC.Areas.Admin.Controllers.Base
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Area("Admin")]
    [Route("admin")]
    [Authorize(Roles = "admin")]
    public abstract class AdminController : Controller
    {
    }
}
