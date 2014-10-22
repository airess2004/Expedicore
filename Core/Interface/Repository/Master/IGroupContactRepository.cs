using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IGroupContactRepository : IRepository<GroupContact>
    {
       IQueryable<GroupContact> GetQueryable();
       GroupContact GetObjectById(int Id);
       GroupContact CreateObject(GroupContact model);
       GroupContact UpdateObject(GroupContact model);
       GroupContact SoftDeleteObject(GroupContact model);
       bool DeleteObject(int Id);
       bool IsNameDuplicated(GroupContact model);
       int GetLastMasterCode(int officeId);
    }
}