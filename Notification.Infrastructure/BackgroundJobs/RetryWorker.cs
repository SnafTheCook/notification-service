using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Notification.Infrastructure.Data;
using Notification.Infrastructure.Services;
using Notification.Domain.Enums;
using Microsoft.EntityFrameworkCore;

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
                    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    var dispatcher = scope.ServiceProvider.GetRequiredService<NotificationDispatcher>();

                    var pendingNotifications = await db.Notifications
                        .Where(n => n.Status == NotificationStatus.AwaitingRetry)
                        .ToListAsync(stoppingToken);

                    foreach (var notification in pendingNotifications)
                    {
                        logger.LogInformation("Attemptin retry for notification {id}", notification.Id);

                        var success = await dispatcher.TryDispatchAsync(notification);

                        if (success)
                            notification.MarkAsSent();
                        else
                            notification.MarkForRetry();
                    }

                    await db.SaveChangesAsync(stoppingToken);
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}
