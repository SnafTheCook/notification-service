using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notification.Domain.Events
{
    public record PetFedEvent(
        Guid petId,
        string PetName,
        string OwnerEmail,
        DateTime FedTime
    );
}
