using Microsoft.AspNetCore.Mvc;
using Notification.Api.Models;
using Notification.Domain.Entities;
using Notification.Domain.Interfaces;
using Notification.Infrastructure.Data;
using Notification.Infrastructure.Services;
using Notification.Infrastructure.Settings;

namespace Notification.Api.Controllers
{
    [ApiController]
    [Route("api/notifications")]
    public class NotificationController(NotificationDispatcher dispatcher, INotificationRepository repository) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> SendNotification([FromBody]SendNotificationRequest request)
        {
            var notification = new NotificationEntity(request.Recipient, request.Content, request.Channel);

            await repository.AddAsync(notification);

            var success = await dispatcher.TryDispatchAsync(notification);

            if (success)
                notification.MarkAsSent();
            else
                notification.MarkForRetry();

            await repository.UpdateAsync(notification);

            return success
                ? Ok(new { Message = "Notification sent successfully", Id = notification.Id })
                : Accepted(new { Message = "Primary providers failed. Queued for retry.", Id = notification.Id });
        }
    }
}
