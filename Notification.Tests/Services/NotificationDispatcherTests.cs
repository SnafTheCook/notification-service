using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Notification.Domain.Entities;
using Notification.Domain.Enums;
using Notification.Domain.Interfaces;
using Notification.Domain.ValueObjects;
using Notification.Infrastructure.Services;
using Notification.Infrastructure.Settings;

namespace Notification.Tests.Services
{
    public class NotificationDispatcherTests
    {
        [Fact]
        public async Task TryDispatchAsync_WhenFirstProviderFails_TriesSecondProvider()
        {
            var mockProvider1 = new Mock<INotificationProvider>();
            var mockProvider2 = new Mock<INotificationProvider>();

            mockProvider1.Setup(p => p.ProviderName).Returns("Twilio");
            mockProvider1.Setup(p => p.SupportedChannel).Returns(ChannelType.Sms);
            mockProvider1.Setup(p => p.SendAsync(It.IsAny<Recipient>(), It.IsAny<string>())).ThrowsAsync(new Exception("API down"));

            mockProvider1.Setup(p => p.ProviderName).Returns("Vonage");
            mockProvider1.Setup(p => p.SupportedChannel).Returns(ChannelType.Sms);
            mockProvider2.Setup(p => p.SendAsync(It.IsAny<Recipient>(), It.IsAny<string>())).ReturnsAsync(true);

            var providers = new List<INotificationProvider> { mockProvider1.Object, mockProvider2.Object };
            var recipient = Recipient.Create("123456789", ChannelType.Sms);

            var settings = Options.Create(new NotificationSettings
            {
                Channels = new List<ChannelConfig>
                {
                    new ChannelConfig
                    {
                        Type = ChannelType.Sms,
                        Providers = new List<ProviderConfig>
                        {
                            new ProviderConfig { Name = "Twilio", Priority = 1, IsEnabled = true },
                            new ProviderConfig { Name = "Vonage", Priority = 2, IsEnabled = true }
                        }
                    }
                }
            });

            var dispatcher = new NotificationDispatcher(providers, settings);
            var note = new NotificationEntity(recipient, "Hello World!", ChannelType.Sms);

            var result = await dispatcher.TryDispatchAsync(note);

            result.Should().BeTrue();

            mockProvider1.Verify(p => p.SendAsync(It.IsAny<Recipient>(), It.IsAny<string>()), Times.Once());
            mockProvider2.Verify(p => p.SendAsync(It.Is<Recipient>(
                r => r.Value == "123456789"), 
                "Hello World!"), 
                Times.Once());
        }
    }
}
