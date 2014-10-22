using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Validation
{
    public interface IContactTypeValidation
    {
        ContactType VCreateObject(ContactType contacttype, IContactTypeService _contacttypeService);
        ContactType VUpdateObject(ContactType contacttype, IContactTypeService _contacttypeService);
    }
}
