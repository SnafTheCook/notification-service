using Microsoft.AspNetCore.Mvc;
using Notification.Api.Models;
using Notification.Domain.Entities;
using Notification.Infrastructure.Data;
using Notification.Infrastructure.Services;
using Notification.Infrastructure.Settings;

namespace Notification.Api.Controllers
{
    [ApiController]
    [Route("api/notifications")]
    public class NotificationController(NotificationDispatcher dispatcher, AppDbContext dbContext) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> SendNotification([FromBody]SendNotificationRequest request)
        {
            var notification = new NotificationEntity(request.Recipient, request.Content, request.Channel);

            dbContext.Notifications.Add(notification);
            await dbContext.SaveChangesAsync();

            var success = await dispatcher.TryDispatchAsync(notification);

            if (success)
                notification.MarkAsSent();
            else
                notification.MarkForRetry();

            await dbContext.SaveChangesAsync();

            return success
                ? Ok(new { Message = "Notification sent successfully", Id = notification.Id })
                : Accepted(new { Message = "Primary providers failed. Queued for retry.", Id = notification.Id });
        }
    }
}
