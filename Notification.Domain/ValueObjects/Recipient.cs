using Notification.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notification.Domain.ValueObjects
{
    public record Recipient
    {
        public string Value { get; }
        private Recipient(string value)
        {
            Value = value;
        }

        public static Recipient Create(string value, ChannelType channelType)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException("Recipient cannot be empty!");

            if (channelType == ChannelType.Email && !value.Contains("@"))
                throw new ArgumentException("Invalid email format.");

            if (channelType == ChannelType.Sms && value.Length < 5)
                throw new ArgumentException("Invalid phone number format");

            return new Recipient(value);
        }

        public static implicit operator string(Recipient recipient) => recipient.Value;
    }
}
