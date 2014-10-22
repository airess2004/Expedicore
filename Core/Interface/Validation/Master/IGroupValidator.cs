using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Validation
{
    public interface IGroupValidation
    {
        Group VCreateObject(Group group, IGroupService _groupService);
        Group VUpdateObject(Group group, IGroupService _groupService);
    }
}
