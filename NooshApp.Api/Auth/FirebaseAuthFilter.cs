using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace NooshApp.Api.Auth
{
    public class FirebaseAuthFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var authHeader = context.HttpContext.Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                context.Result = new UnauthorizedObjectResult(new { message = "Missing or invalid Authorization header." });
                return;
            }

            var idToken = authHeader.Substring("Bearer ".Length);
            try
            {
                var decoded = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(idToken);
                if (!decoded.Claims.TryGetValue("email", out var email))
                {
                    context.Result = new UnauthorizedObjectResult(new { message = "Token has no verified email." });
                    return;
                }
                context.HttpContext.Items["VerifiedEmail"] = email.ToString();
            }
            catch (Exception)
            {
                context.Result = new UnauthorizedObjectResult(new { message = "Invalid or expired token." });
                return;
            }
            await next();
        }
    }
}