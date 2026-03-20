using Notification.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notification.Domain.Interfaces
{
    public interface INotificationRepository
    {
        Task TaskAsync(NotificationEntity notification);
        Task UpdateAsync(NotificationEntity notification);
        Task<NotificationEntity> GetByIdAsync(Guid id);
        Task<IEnumerable<NotificationEntity>> GetPendingRetryAsync();
    }
}
