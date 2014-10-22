using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IGroupContactService
    {
        IQueryable<GroupContact> GetQueryable();
        GroupContact GetObjectById(int Id);
        GroupContact CreateObject(GroupContact groupcontact);
        GroupContact UpdateObject(GroupContact groupcontact);
        GroupContact SoftDeleteObject(GroupContact groupcontact);
        bool IsNameDuplicated(GroupContact groupcontact);
    }
}