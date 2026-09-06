using Notification.Domain.Common;
using Notification.Domain.Enums;
using Notification.Domain.Exceptions;
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

        public static Result<Recipient> Create(string value, ChannelType channelType)
        {
            if (string.IsNullOrWhiteSpace(value))
                return Result<Recipient>.Failure(new Error("Recipient.Empty", "Recipient cannot be empty!"));

            if (channelType == ChannelType.Email && !value.Contains("@"))
                return Result<Recipient>.Failure(new Error("Recipient.InvalidEmail", "Invalid email format."));

            if (channelType == ChannelType.Sms && value.Length < 5)
                return Result<Recipient>.Failure(new Error("Recipient.InvalidPhoneNumber", "Invalid phone number format"));

            return Result<Recipient>.Success(new Recipient(value));
        }

        public static implicit operator string(Recipient recipient) => recipient.Value;
    }
}
