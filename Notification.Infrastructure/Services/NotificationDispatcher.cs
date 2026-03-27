using Microsoft.Extensions.Options;
using Notification.Domain.Interfaces;
using Notification.Domain.Entities;
using Notification.Infrastructure.Settings;
using Polly;
using Polly.Retry;
using Microsoft.Extensions.Logging;

namespace Notification.Infrastructure.Services
{
    public class NotificationDispatcher (IEnumerable<INotificationProvider> providers, IOptions<NotificationSettings> settings, ILogger<NotificationDispatcher> _logger)
    {
        private readonly ResiliencePipeline<bool> _retryPipeline = new ResiliencePipelineBuilder<bool>()
            .AddRetry(new RetryStrategyOptions<bool>
            {
                ShouldHandle = new PredicateBuilder<bool>().Handle<Exception>(),
                BackoffType = DelayBackoffType.Exponential,
                MaxRetryAttempts = 2,
                Delay = TimeSpan.FromSeconds(1)
            })
            .Build();

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
                    var result = await _retryPipeline.ExecuteAsync(async token =>
                    {
                        return await provider.SendAsync(notification.Recipient, notification.Content);
                    });

                    if (result)
                    {
                        notification.MarkAsSent(provider.ProviderName);
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Provider {0} failed after retries. Attempting failover", provider.ProviderName);
                }
            }

            notification.MarkForRetry();
            return false;
        }
    }
}
