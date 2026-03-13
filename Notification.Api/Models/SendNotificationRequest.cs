using Notification.Domain.Enums;

namespace Notification.Api.Models
{
    public record SendNotificationRequest(string Recipient, string Content, ChannelType Channel);
}
