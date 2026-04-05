using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Notification.Domain.Wrappers;

namespace Notification.Api.Filters
{
    public class ValidationFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                context.Result = new BadRequestObjectResult(ApiResponse.Fail(string.Join(" | ", errors)));
                return;
            }
            await next();
        }
    }
}
