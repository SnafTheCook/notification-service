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
        private readonly Mock<ILogger<NotificationService>> _mockLogger = new();
        private readonly NotificationService _notificationService;

        public NotificationServiceTests()
        {
            _notificationService = new NotificationService(_mockRepo.Object, _mockDispatcher.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task ProcessNotification_Success_PersistsAndUpdates()
        {
            var recipient = "test@test.com";
            var content = "TestMessage";
            _mockDispatcher.Setup(d => d.TryDispatchAsync(It.IsAny<NotificationEntity>())).ReturnsAsync(true);

            await _notificationService.ProcessNotificationAsync(recipient, content, ChannelType.Email);

            _mockRepo.Verify(r => r.AddAsync(It.IsAny<NotificationEntity>()), Times.Once);
            _mockRepo.Verify(r => r.UpdateAsync(It.Is<NotificationEntity>(n => n.Status == NotificationStatus.Sent), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
