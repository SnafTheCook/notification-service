using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Notification.Infrastructure.Data;
using Notification.Infrastructure.Services;
using Notification.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Notification.Domain.Interfaces;

namespace Notification.Infrastructure.BackgroundJobs
{
    public class RetryWorker(IServiceScopeFactory scopeFactory, ILogger<RetryWorker> logger) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("Retry worker started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = scopeFactory.CreateScope())
                {
                    var repository = scope.ServiceProvider.GetRequiredService<INotificationRepository>();
                    var dispatcher = scope.ServiceProvider.GetRequiredService<NotificationDispatcher>();

                    var pendingNotifications = await repository.GetPendingRetryAsync();

                    foreach (var notification in pendingNotifications)
                    {
                        logger.LogInformation("Attemptin retry for notification {id}", notification.Id);

                        var success = await dispatcher.TryDispatchAsync(notification);

                        if (success)
                            notification.MarkAsSent();
                        else
                            notification.MarkForRetry();

                        await repository.UpdateAsync(notification);
                    }
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}
