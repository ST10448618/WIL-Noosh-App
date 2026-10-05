using Microsoft.AspNetCore.Http;

namespace NooshApp.Web.Helpers
{
    public static class StaffSessionExtensions
    {
        private const string StaffPinKey = "StaffPinVerified";

        public static void SetStaffPin(this ISession session, string pin) => session.SetString(StaffPinKey, pin);
        public static string? GetStaffPin(this ISession session) => session.GetString(StaffPinKey);
        public static bool IsStaffAuthenticated(this ISession session) => !string.IsNullOrEmpty(session.GetString(StaffPinKey));
        public static void ClearStaffPin(this ISession session) => session.Remove(StaffPinKey);
    }
}