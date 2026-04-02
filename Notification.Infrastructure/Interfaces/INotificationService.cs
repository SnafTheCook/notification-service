using Notification.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notification.Infrastructure.Interfaces
{
    public interface INotificationService
    {
        Task ProcessNotificationAsync(string recipient, string content, ChannelType channel);
    }
}
