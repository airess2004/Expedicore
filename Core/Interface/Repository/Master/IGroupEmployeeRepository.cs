using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IGroupEmployeeRepository : IRepository<GroupEmployee>
    {
       IQueryable<GroupEmployee> GetQueryable();
       GroupEmployee GetObjectById(int Id);
       GroupEmployee CreateObject(GroupEmployee model);
       GroupEmployee UpdateObject(GroupEmployee model);
       GroupEmployee SoftDeleteObject(GroupEmployee model);
       bool DeleteObject(int Id);
       bool IsNameDuplicated(GroupEmployee model);
       int GetLastMasterCode(int officeId);
    }
}