using Notification.Domain.Enums;
using Notification.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notification.Infrastructure.Providers
{
    public class AmazonSnsEmailProvider : INotificationProvider
    {
        public string ProviderName => "AmazonSNS";

        public ChannelType SupportedChannel => ChannelType.Email;

        public async Task<bool> SendAsync(string recipient, string content)
        {
            await Task.Delay(100);
            return true;
        }
    }
}
