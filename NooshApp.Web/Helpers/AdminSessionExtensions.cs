using Microsoft.AspNetCore.Http;

namespace NooshApp.Web.Helpers
{
    public static class AdminSessionExtensions
    {
        private const string AdminKeyKey = "AdminKeyVerified";
        public static void SetAdminKey(this ISession session, string key) => session.SetString(AdminKeyKey, key);
        public static string? GetAdminKey(this ISession session) => session.GetString(AdminKeyKey);
        public static bool IsAdminAuthenticated(this ISession session) => !string.IsNullOrEmpty(session.GetString(AdminKeyKey));
        public static void ClearAdminKey(this ISession session) => session.Remove(AdminKeyKey);
    }
}