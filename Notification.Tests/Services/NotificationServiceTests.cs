using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using Moq;
using Notification.Domain.Interfaces;
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
        private readonly Mock<NotificationDispatcher> _mockDispatcher;
        private readonly Mock<ILogger<NotificationService>> _mockLogger = new();
        private readonly NotificationService _notificationService;

        public NotificationServiceTests()
        {
            _mockDispatcher = new Mock<NotificationDispatcher>(new List<INotificationProvider>(), null!, null!);
            _notificationService = new NotificationService(_mockRepo.Object, _mockDispatcher.Object, _mockLogger.Object);
        }
    }
}
