using Notification.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notification.Domain.DTOs
{
    public record NotificationResponseDTO(
        Guid id,
        string Recipient,
        string Content,
        ChannelType Channel,
        NotificationStatus Status,
        string? SucceededProvider,
        DateTime CreatedAt,
        DateTime? LastModifiedAt
        );
}
