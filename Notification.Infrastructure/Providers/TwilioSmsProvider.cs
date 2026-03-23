using Notification.Domain.Enums;
using Notification.Domain.Interfaces;
using Notification.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notification.Infrastructure.Providers
{
    public class TwilioSmsProvider : INotificationProvider
    {
        public string ProviderName => "Twilio";

        public ChannelType SupportedChannel => ChannelType.Sms;

        public async Task<bool> SendAsync(Recipient recipient, string content)
        {
            await Task.Delay(100);
            return true;
        }
    }
}
