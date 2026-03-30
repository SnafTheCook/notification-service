using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notification.Domain.ValueObjects
{
    public record MessageContent
    {
        public string Value { get; }
        private MessageContent(string value) => Value = value;

        public static MessageContent Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Content empty!");
            return new MessageContent(value);
        }

        public static implicit operator string(MessageContent content) => content.Value;
    }
}
