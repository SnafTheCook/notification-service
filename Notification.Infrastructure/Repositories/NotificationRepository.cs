using Microsoft.EntityFrameworkCore;
using Notification.Domain.Entities;
using Notification.Domain.Enums;
using Notification.Domain.Interfaces;
using Notification.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notification.Infrastructure.Repositories
{
    public class NotificationRepository(AppDbContext context) : INotificationRepository
    {
        public async Task<NotificationEntity?> GetByIdAsync(Guid id)
        {
            return await context.Notifications.FindAsync(id);
        }

        public async Task<IEnumerable<NotificationEntity>> GetPendingRetryAsync(CancellationToken ct = default)
        {
            return await context.Notifications
                .Where(n => n.Status == NotificationStatus.AwaitingRetry)
                .ToListAsync(ct);
        }

        public async Task AddAsync(NotificationEntity notification)
        {
            await context.Notifications.AddAsync(notification);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(NotificationEntity notification, CancellationToken ct = default)
        {
            context.Notifications.Update(notification);
            await context.SaveChangesAsync(ct);
        }
    }
}
