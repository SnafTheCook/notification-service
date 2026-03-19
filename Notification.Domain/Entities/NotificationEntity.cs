using Notification.Domain.Enums;
using Notification.Domain.ValueObjects;
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
        public Recipient Recipient { get; private set; }
        public string Content { get; private set; }
        public ChannelType Channel { get; private set; }
        public NotificationStatus Status { get; private set; }
        public int AttemptCount { get; private set; }

        public NotificationEntity(Recipient recipient, string content, ChannelType channel)
        {
            Id = Guid.NewGuid();
            Recipient = Recipient.Create(recipient, channel);
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
