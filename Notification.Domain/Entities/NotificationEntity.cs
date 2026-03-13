using Notification.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notification.Domain.Entities
{
    public class NotificationEntity
    {
        public Guid Id { get; private set; }
        public string Recipient { get; private set; }
        public string Content { get; private set; }
        public ChannelType Channel { get; private set; }
        public NotificationStatus Status { get; private set; }
        public int AttemptCount { get; private set; }

        public NotificationEntity(string recipient, string content, ChannelType channel)
        {
            Id = Guid.NewGuid();
            Recipient = recipient;
            Content = content;
            Channel = channel;
            Status = NotificationStatus.Pending;
        }

        public void MarkAsSent() => Status = NotificationStatus.Sent;
        public void MarkForRetry()
        {
            Status = NotificationStatus.AwaitingRetry;
            AttemptCount++;
        }
        public void MarkAsFailed() => Status = NotificationStatus.Failed;
    }
}
