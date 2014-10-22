using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IGroupService
    {
        IQueryable<Group> GetQueryable();
        Group GetObjectById(int Id);
        Group CreateObject(Group group);
        Group UpdateObject(Group group);
        Group SoftDeleteObject(Group group);
        bool IsNameDuplicated(Group group);
    }
}