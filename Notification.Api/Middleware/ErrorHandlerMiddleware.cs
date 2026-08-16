using Notification.Domain.Exceptions;
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
                logger.LogError(ex, "An unhandled exception occured");

                var (statusCode, errorMessage) = ex switch
                {
                    DomainException => (StatusCodes.Status400BadRequest, ex.Message),
                    _ => (StatusCodes.Status500InternalServerError, "Internal server error")
                };

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = statusCode;

                var id = context.Request.Headers["X-Correlation-ID"].FirstOrDefault();
                await context.Response.WriteAsJsonAsync(new
                {
                    Success = false,
                    Error = errorMessage,
                    CorrelationId = id
                });
            }
        }
    }
}
