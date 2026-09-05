using Notification.Domain.Common;
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

        public static Result<MessageContent> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) 
                return Result<MessageContent>.Failure(new Error("Content.Empty", "Content empty!"));

            return Result<MessageContent>.Success(new MessageContent(value));
        }

        public static implicit operator string(MessageContent content) => content.Value;
    }
}
