using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Validation
{
    public interface IContactValidation
    {
        Contact VCreateObject(Contact contact, IContactService _contactService);
        Contact VUpdateObject(Contact contact, IContactService _contactService);
    }
}
