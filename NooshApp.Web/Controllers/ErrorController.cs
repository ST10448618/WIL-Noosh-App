using Microsoft.AspNetCore.Mvc;

namespace NooshApp.Web.Controllers
{
    public class ErrorController : Controller
    {
        [Route("NotFound")]
        public IActionResult NotFoundPage() => View("NotFound");
    }
}