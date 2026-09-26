using Microsoft.AspNetCore.Mvc;
using NooshApp.Web.Services;
using NooshApp.Web.ViewModels;

namespace NooshApp.Web.Controllers
{
    public class CareersController : Controller
    {
        private readonly ICareersApiClient _careersApiClient;
        public CareersController(ICareersApiClient careersApiClient) { _careersApiClient = careersApiClient; }

    }
}