using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Validation
{
    public interface IGroupContactValidation
    {
        GroupContact VCreateObject(GroupContact groupcontact, IGroupContactService _groupcontactService);
        GroupContact VUpdateObject(GroupContact groupcontact, IGroupContactService _groupcontactService);
    }
}
