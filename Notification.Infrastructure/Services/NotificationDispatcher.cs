using Microsoft.Extensions.Options;
using Notification.Domain.Interfaces;
using Notification.Domain.Entities;
using Notification.Infrastructure.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notification.Infrastructure.Services
{
    public class NotificationDispatcher (IEnumerable<INotificationProvider> providers, IOptions<NotificationSettings> settings)
    {
        public async Task<bool> TryDispatchAsync(NotificationEntity notification)
        {
            var channelConfig = settings.Value.Channels
                .FirstOrDefault(c => c.Type == notification.Channel);

            if (channelConfig == null) return false;

            var orderedProvidersConfig = channelConfig.Providers
                .Where(p => p.IsEnabled)
                .OrderBy(p => p.Priority);

            foreach (var config in orderedProvidersConfig)
            {
                var provider = providers.FirstOrDefault(p => string.Equals(p.ProviderName, config.Name));
                if (provider == null) continue;

                try
                {
                    var success = await provider.SendAsync(notification.Recipient, notification.Content);
                    if (success) return true;
                }
                catch
                {
                    //try next provider
                }
            }

            return false;
        }
    }
}
