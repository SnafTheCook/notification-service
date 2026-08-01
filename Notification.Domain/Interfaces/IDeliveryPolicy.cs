using Notification.Domain.Entities;

namespace Notification.Domain.Interfaces
{
    public interface IDeliveryPolicy
    {
        bool CanSend(NotificationEntity notification, IEnumerable<NotificationEntity> history);
    }
}
