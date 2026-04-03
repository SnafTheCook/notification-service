using Microsoft.AspNetCore.Mvc;
using Notification.Api.Models;
using Notification.Domain.Entities;
using Notification.Domain.Interfaces;
using Notification.Infrastructure.Data;
using Notification.Infrastructure.Interfaces;
using Notification.Infrastructure.Services;
using Notification.Infrastructure.Settings;

namespace Notification.Api.Controllers
{
    [ApiController]
    [Route("api/notifications")]
    public class NotificationController(INotificationService notificationService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> SendNotification([FromBody]SendNotificationRequest request)
        {
            await notificationService.ProcessNotificationAsync(request.Recipient, request.Content, request.Channel);

            return Ok(new { Message = "Request accepted and processing started." });
        }
    }
}
