using Microsoft.AspNetCore.Http;

namespace NooshApp.Web.Helpers
{
    public static class SessionExtensions
    {
        private const string EmailKey = "LoggedInEmail";
        private const string IdTokenKey = "FirebaseIdToken";

        public static void SetLoggedInCustomer(this ISession session, string email, string idToken)
        {
            session.SetString(EmailKey, email);
            session.SetString(IdTokenKey, idToken);
        }
        public static string? GetLoggedInEmail(this ISession session) => session.GetString(EmailKey);
        public static string? GetIdToken(this ISession session) => session.GetString(IdTokenKey);
        public static bool IsLoggedIn(this ISession session) => !string.IsNullOrEmpty(session.GetString(EmailKey));
        public static void ClearLoggedInCustomer(this ISession session)
        {
            session.Remove(EmailKey);
            session.Remove(IdTokenKey);
        }
    }
}