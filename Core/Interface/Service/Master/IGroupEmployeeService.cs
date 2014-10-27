using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IGroupEmployeeService
    {
        IQueryable<GroupEmployee> GetQueryable();
        GroupEmployee GetObjectById(int Id);
        GroupEmployee CreateObject(GroupEmployee groupemployee);
        GroupEmployee UpdateObject(GroupEmployee groupemployee);
        GroupEmployee SoftDeleteObject(GroupEmployee groupemployee);
        bool IsNameDuplicated(GroupEmployee groupemployee);
    }
}