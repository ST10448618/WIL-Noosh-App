using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace NooshApp.Api.Auth
{
    public class StaffPinFilter : IActionFilter
    {
        private readonly IConfiguration _configuration;
        public StaffPinFilter(IConfiguration configuration) { _configuration = configuration; }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var providedPin = context.HttpContext.Request.Headers["X-Staff-Pin"].ToString();
            var actualPin = _configuration["Staff:ScanPin"];
            if (string.IsNullOrEmpty(providedPin) || providedPin != actualPin)
                context.Result = new UnauthorizedObjectResult(new { message = "Invalid staff PIN." });
        }
        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}