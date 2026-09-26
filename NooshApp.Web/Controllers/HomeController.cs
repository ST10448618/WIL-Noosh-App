using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NooshApp.Web.Models;
using NooshApp.Web.ViewModels;

namespace NooshApp.Web.Controllers;

public class HomeController : Controller
{

    private readonly IMenuApiClient _menuApiClient;
    public HomeController(IMenuApiClient menuApiClient) { _menuApiClient = menuApiClient; }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
