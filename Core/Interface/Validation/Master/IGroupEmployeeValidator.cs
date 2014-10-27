using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Validation
{
    public interface IGroupEmployeeValidation
    {
        GroupEmployee VCreateObject(GroupEmployee groupemployee, IGroupEmployeeService _groupemployeeService);
        GroupEmployee VUpdateObject(GroupEmployee groupemployee, IGroupEmployeeService _groupemployeeService);
    }
}
