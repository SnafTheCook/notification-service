using Notification.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notification.Domain.Interfaces
{
    public interface INotificationProvider
    {
        string ProviderName { get; }
        ChannelType SupportedChannel { get; }
        Task<bool> SendAsync(string recipient, string content);
    }
}
