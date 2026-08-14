using MassTransit;
using Notification.Domain.Enums;
using Notification.Domain.Events;
using Notification.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notification.Infrastructure.Consumers
{
    public class PetFedConsumer(INotificationService notificationService) : IConsumer<PetFedEvent>
    {
        public async Task Consume(ConsumeContext<PetFedEvent> context)
        {
            var correlationId = context.CorrelationId ?? Guid.NewGuid();

            await notificationService.ProcessNotificationAsync(
                context.Message.OwnerEmail,
                $"Your pet {context.Message.PetName} was just fed!",
                ChannelType.Email,
                correlationId);
        }
    }
}
