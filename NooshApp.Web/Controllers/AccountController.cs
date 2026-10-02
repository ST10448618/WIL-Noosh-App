using Microsoft.AspNetCore.Mvc;
using NooshApp.Web.Helpers;

namespace NooshApp.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConfiguration _configuration;
        public AccountController(IConfiguration configuration) { _configuration = configuration; }

        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.IsLoggedIn())
                return RedirectToAction("Index", "Rewards");

            ViewBag.FirebaseApiKey = _configuration["Firebase:ApiKey"];
            ViewBag.FirebaseAuthDomain = _configuration["Firebase:AuthDomain"];
            ViewBag.FirebaseProjectId = _configuration["Firebase:ProjectId"];
            return View();
        }

        [HttpPost]
        public IActionResult CompleteLogin(string email, string idToken, string? fullName)
        {
            HttpContext.Session.SetLoggedInCustomer(email, idToken);
            return Ok();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.ClearLoggedInCustomer();
            return RedirectToAction("Index", "Home");
        }
    }
}