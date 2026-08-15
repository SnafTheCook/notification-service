using Serilog.Context;

namespace Notification.Api.Middleware
{
    public class CorrelationIdMiddleware(RequestDelegate next)
    {
        public async Task Invoke(HttpContext context)
        {
            var id = context.Request.Headers["X-Correlation-ID"].FirstOrDefault()
                     ?? Guid.NewGuid().ToString();

            using (LogContext.PushProperty("CorrelationId", id))
            {
                await next(context);
            }
        }
    }
}
