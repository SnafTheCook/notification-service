using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using Moq;
using Notification.Domain.Entities;
using Notification.Domain.Enums;
using Notification.Domain.Interfaces;
using Notification.Infrastructure.Interfaces;
using Notification.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notification.Tests.Services
{
    public class NotificationServiceTests
    {
        private readonly Mock<INotificationRepository> _mockRepo = new();
        private readonly Mock<INotificationDispatcher> _mockDispatcher = new();
        private readonly Mock<IDeliveryPolicy> _mockPolicy = new();
        private readonly Mock<ILogger<NotificationService>> _mockLogger = new();
        private readonly NotificationService _notificationService;

        public NotificationServiceTests()
        {
            _notificationService = new NotificationService(_mockRepo.Object, _mockDispatcher.Object, _mockPolicy.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task ProcessNotification_Success_PersistsAndUpdates()
        {
            _mockPolicy.Setup(p => p.CanSend(It.IsAny<NotificationEntity>(), It.IsAny<IEnumerable<NotificationEntity>>()))
              .Returns(true);

            _mockDispatcher
                .Setup(d => d.TryDispatchAsync(It.IsAny<NotificationEntity>(), default))
                .Callback<NotificationEntity, CancellationToken>((n, _) => n.MarkAsSent("TestProvider"))
                .ReturnsAsync(true);

            await _notificationService.ProcessNotificationAsync("test@test.com", "Hello", ChannelType.Email, Guid.NewGuid(), default);

            _mockDispatcher.Verify(d => d.TryDispatchAsync(It.IsAny<NotificationEntity>(), default), Times.Once);

            _mockRepo.Verify(r => r.UpdateAsync(
                It.Is<NotificationEntity>(n => n.Status == NotificationStatus.Sent),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task ProcessNotification_WhenPolicyAllows_PersistsAndDispatches()
        {
            var recipient = "test@test.com";

            _mockPolicy.Setup(p => p.CanSend(It.IsAny<NotificationEntity>(), It.IsAny<IEnumerable<NotificationEntity>>()))
                       .Returns(true);

            _mockDispatcher.Setup(d => d.TryDispatchAsync(It.IsAny<NotificationEntity>(), default))
                           .Callback<NotificationEntity, CancellationToken>((n, _) => n.MarkAsSent("TestProvider"))
                           .ReturnsAsync(true);

            await _notificationService.ProcessNotificationAsync(recipient, "Hello", ChannelType.Email, Guid.NewGuid(), default);

            _mockDispatcher.Verify(d => d.TryDispatchAsync(It.IsAny<NotificationEntity>(), default), Times.Once);

            _mockRepo.Verify(r => r.UpdateAsync(
                It.Is<NotificationEntity>(n => n.Status == NotificationStatus.Sent),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task ProcessNotification_WhenPolicyBlocks_DoesNotDispatch()
        {
            _mockPolicy.Setup(p => p.CanSend(It.IsAny<NotificationEntity>(), It.IsAny<IEnumerable<NotificationEntity>>()))
                       .Returns(false);

            await _notificationService.ProcessNotificationAsync("test@test.com", "Hello", ChannelType.Email, Guid.NewGuid(), default);

            _mockDispatcher.Verify(d => d.TryDispatchAsync(It.IsAny<NotificationEntity>(), default), Times.Never);

            _mockRepo.Verify(r => r.UpdateAsync(It.Is<NotificationEntity>(n => n.Status == NotificationStatus.Failed), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
