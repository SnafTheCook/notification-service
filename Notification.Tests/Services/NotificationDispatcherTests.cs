using Castle.Core.Logging;
using FluentAssertions;
using Microsoft.Extensions.Logging;
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

            mockProvider2.Setup(p => p.ProviderName).Returns("Vonage");
            mockProvider2.Setup(p => p.SupportedChannel).Returns(ChannelType.Sms);
            mockProvider2.Setup(p => p.SendAsync(It.IsAny<Recipient>(), It.IsAny<string>())).ReturnsAsync(true);

            var providers = new List<INotificationProvider> { mockProvider1.Object, mockProvider2.Object };

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

            var mockLogger = new Mock<ILogger<NotificationDispatcher>>();

            var dispatcher = new NotificationDispatcher(providers, settings, mockLogger.Object);
            var note = new NotificationEntity("123456789", "Hello World!", ChannelType.Sms);

            var result = await dispatcher.TryDispatchAsync(note);

            result.Should().BeTrue();

            mockProvider1.Verify(p => p.SendAsync(It.IsAny<Recipient>(), It.IsAny<string>()), Times.Exactly(3));
            mockProvider2.Verify(p => p.SendAsync(It.Is<Recipient>(
                r => r.Value == "123456789"), 
                "Hello World!"), 
                Times.Once());
        }

        [Fact]
        public async Task TryDispatchAsync_WhenAllProvidersFail_ReturnsFalse()
        {
            var providerMock = new Mock<INotificationProvider>();
            providerMock.Setup(p => p.ProviderName).Returns("Twilio");
            providerMock.Setup(p => p.SupportedChannel).Returns(ChannelType.Sms);
            providerMock.Setup(p => p.SendAsync(It.IsAny<Recipient>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception("Failed reaching provider"));

            var settings = Options.Create(new NotificationSettings
            {
                Channels = new List<ChannelConfig>
                {
                    new ChannelConfig
                    {
                        Type = ChannelType.Sms,
                        Providers = new List<ProviderConfig>
                        {
                            new ProviderConfig { Name = "Twilio", Priority = 1, IsEnabled = true }
                        }
                    }
                }
            });

            var mockLogger = new Mock<ILogger<NotificationDispatcher>>();

            var dispatcher = new NotificationDispatcher(new List<INotificationProvider> { providerMock.Object }, settings, mockLogger.Object);
            var note = new NotificationEntity("123456789", "test", ChannelType.Sms);

            var result = await dispatcher.TryDispatchAsync(note);

            result.Should().BeFalse();
            note.Status.Should().NotBe(NotificationStatus.Sent);
        }
    }
}
