using Microsoft.AspNetCore.Mvc;
using NooshApp.Web.Services;
using NooshApp.Web.ViewModels;

namespace NooshApp.Web.Controllers
{
    public class MenuController : Controller
    {
        private readonly IMenuApiClient _menuApiClient;
        public MenuController(IMenuApiClient menuApiClient) { _menuApiClient = menuApiClient; }

        public async Task<IActionResult> Index()
        {
            var items = await _menuApiClient.GetAllAsync();

            var viewModel = new MenuViewModel
            {
                Items = items,
                Categories = items.Select(i => i.Category).Distinct().OrderBy(c => c).ToList()
            };

            return View(viewModel);
        }
    }
}