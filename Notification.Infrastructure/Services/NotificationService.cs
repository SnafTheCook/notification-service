using Microsoft.Extensions.Logging;
using Notification.Domain.Entities;
using Notification.Domain.Enums;
using Notification.Domain.Interfaces;
using Notification.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notification.Infrastructure.Services
{
    public class NotificationService(
        INotificationRepository repository,
        INotificationDispatcher dispatcher,
        ILogger<NotificationService> logger) : INotificationService
    {
        public async Task ProcessNotificationAsync(string recipient, string content, ChannelType channel)
        {
            var notification = new NotificationEntity(recipient, content, channel);

            await repository.AddAsync(notification);
            await dispatcher.TryDispatchAsync(notification);
            await repository.UpdateAsync(notification);

            logger.LogInformation("Processed notification {id}. Created at: {createdAt}. Status: {status}.",
                notification.Id,
                notification.CreatedAt,
                notification.Status);
        }
    }
}
