using FluentValidation;
using Notification.Api.Models;

namespace Notification.Api.Validators
{
    public class SendNotificationRequestValidator : AbstractValidator<SendNotificationRequest>
    {
        public SendNotificationRequestValidator() 
        {
            RuleFor(x => x.Recipient.Value).NotEmpty().MinimumLength(5);
            RuleFor(x => x.Content).NotEmpty().MaximumLength(500);
            RuleFor(x => x.Channel).IsInEnum();
        }
    }
}
