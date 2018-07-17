namespace AnteyaSidOnContainers.WebApps.WebMVC.Areas.Admin.Controllers.Base
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;


    //[Authorize]
    [Area("Admin")]
    [Route("admin/[controller]/[action]")]
    //[Authorize(Roles = "admin")]
    public abstract class AdminController : Controller
    {
    }
}
