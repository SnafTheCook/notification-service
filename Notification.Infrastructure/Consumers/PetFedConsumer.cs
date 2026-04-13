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
            var @event = context.Message;

            await notificationService.ProcessNotificationAsync(
                @event.OwnerEmail,
                $"Your pet {@event.PetName} was just fed!",
                ChannelType.Email);
        }
    }
}
