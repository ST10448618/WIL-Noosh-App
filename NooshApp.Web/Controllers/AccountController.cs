using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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
            ViewBag.FirebaseApiKey = _configuration["Firebase:ApiKey"];
            ViewBag.FirebaseAuthDomain = _configuration["Firebase:AuthDomain"];
            ViewBag.FirebaseProjectId = _configuration["Firebase:ProjectId"];
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CompleteLogin(string email, string idToken, string? fullName)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, email),
                new Claim("FirebaseIdToken", idToken)
            };
            if (!string.IsNullOrEmpty(fullName))
                claims.Add(new Claim(ClaimTypes.Name, fullName));

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

            return Ok();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}