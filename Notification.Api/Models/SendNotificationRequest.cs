using Notification.Domain.Enums;
using Notification.Domain.ValueObjects;

namespace Notification.Api.Models
{
    public record SendNotificationRequest(Recipient Recipient, string Content, ChannelType Channel);
}
