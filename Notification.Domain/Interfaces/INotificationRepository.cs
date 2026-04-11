using Notification.Domain.Entities;
using Notification.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notification.Domain.Interfaces
{
    public interface INotificationRepository
    {
        Task AddAsync(NotificationEntity notification);
        Task UpdateAsync(NotificationEntity notification, CancellationToken ct = default);
        Task<IEnumerable<NotificationEntity>> GetAllAsync(NotificationStatus? status, ChannelType? channel);
        Task<NotificationEntity?> GetByIdAsync(Guid id);
        Task<IEnumerable<NotificationEntity>> GetPendingRetryAsync(CancellationToken ct = default);
    }
}
