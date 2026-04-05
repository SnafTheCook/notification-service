using Notification.Domain.Wrappers;

namespace Notification.Api.Middleware
{
    public class ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
    {
        public async Task Invoke(HttpContext context)
        {
            try 
            { 
                await next(context); 
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 500;
                await context.Response.WriteAsJsonAsync(ApiResponse.Fail("Internal Server Error"));
            }
        }
    }
}
