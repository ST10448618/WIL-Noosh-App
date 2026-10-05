using System.Security.Claims;

namespace NooshApp.Web.Helpers
{
    public static class AuthHelpers
    {
        public static bool IsLoggedIn(this HttpContext context) =>
            context.User.Identity?.IsAuthenticated == true;

        public static string? GetEmail(this HttpContext context) =>
            context.User.FindFirst(ClaimTypes.Email)?.Value;

        public static string? GetIdToken(this HttpContext context) =>
            context.User.FindFirst("FirebaseIdToken")?.Value;
    }
}