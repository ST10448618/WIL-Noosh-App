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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Request(CateringRequestViewModel model)
        {
            if (model.EventDate.Date < DateTime.Today)
                ModelState.AddModelError(nameof(model.EventDate), "Event date cannot be in the past.");

            if (!ModelState.IsValid) return View(model);

            var dto = new CateringRequestCreateDto
            {
                FullName = model.FullName,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                EventDate = model.EventDate,
                GuestCount = model.GuestCount,
                EventLocation = model.EventLocation,
                AdditionalNotes = model.AdditionalNotes
            };

            var result = await _cateringApiClient.SubmitAsync(dto);
            return RedirectToAction("Confirmation", new { id = result.Id });
        }

        public async Task<IActionResult> Confirmation(int id)
        {
            var result = await _cateringApiClient.GetByIdAsync(id);
            if (result == null) return NotFound();

            ViewBag.RequestId = result.Id;
            return View();
        }
    }
}