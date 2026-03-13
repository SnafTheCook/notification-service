using Notification.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notification.Infrastructure.Settings
{
    public class NotificationSettings
    {
        public List<ChannelConfig> Channels { get; set; } = [];
    }

    public class ChannelConfig
    {
        public ChannelType Type { get; set; }
        public List<ProviderConfig> Providers { get; set; } = [];
    }

    public class ProviderConfig
    {
        public string Name { get; set; } = string.Empty;
        public int Priority { get; set; }
        public bool IsEnabled { get; set; }
    }
}
