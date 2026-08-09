using Notification.Domain.Entities;
using Notification.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notification.Domain.Services
{
    public class RateLimitPolicy : IDeliveryPolicy
    {
        public bool CanSend(NotificationEntity notification, IEnumerable<NotificationEntity> history)
        {
            return !history.Any(h =>
                h.Recipient.Value == notification.Recipient.Value &&
                h.Status == Enums.NotificationStatus.Sent &&
                h.SentAt > DateTime.UtcNow.AddMinutes(-1));
        }
    }
}
