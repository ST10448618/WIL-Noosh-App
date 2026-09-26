using Microsoft.AspNetCore.Mvc;
using NooshApp.Web.Dtos;
using NooshApp.Web.Services;
using NooshApp.Web.ViewModels;

namespace NooshApp.Web.Controllers
{
    public class CateringController : Controller
    {
        private readonly ICateringApiClient _cateringApiClient;
        public CateringController(ICateringApiClient cateringApiClient) { _cateringApiClient = cateringApiClient; }

        [HttpGet]
        public IActionResult Request() => View(new CateringRequestViewModel());
    }
}