using Eaf.Auditing;
using Eaf.Middleware.Web.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Eaf.Str.Web.Controllers
{
    public class HomeController : MiddlewareControllerBase
    {
        [DisableAuditing]
        public IActionResult Index()
        {
            return Redirect($"/swagger");
        }
    }
}
