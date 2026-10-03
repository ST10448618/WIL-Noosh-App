using Microsoft.AspNetCore.Mvc;
using NooshApp.Web.Services;
using NooshApp.Web.ViewModels;

namespace NooshApp.Web.Controllers
{
    public class CareersController : Controller
    {
        private readonly ICareersApiClient _careersApiClient;
        public CareersController(ICareersApiClient careersApiClient) { _careersApiClient = careersApiClient; }

        [HttpGet]
        public IActionResult Apply() => View(new CareerApplicationViewModel());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Apply(CareerApplicationViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _careersApiClient.SubmitApplicationAsync(model);

            if (!result.Success)
            {
                ModelState.AddModelError("CvFile", result.ErrorMessage ?? "Submission failed.");
                return View(model);
            }

            return RedirectToAction("Confirmation", new { id = result.Id });
        }

        public IActionResult Confirmation(int id)
        {
            ViewBag.ApplicationId = id;
            return View();
        }
    }
}