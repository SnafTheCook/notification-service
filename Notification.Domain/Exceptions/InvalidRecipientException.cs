using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notification.Domain.Exceptions
{
    public class InvalidRecipientException(string message) : DomainException(message)
    {

    }
}
