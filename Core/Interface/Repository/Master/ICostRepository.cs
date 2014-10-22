using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface ICostRepository : IRepository<Cost>
    {
       IQueryable<Cost> GetQueryable();
       Cost GetObjectById(int Id);
       Cost CreateObject(Cost model);
       Cost UpdateObject(Cost model);
       Cost SoftDeleteObject(Cost model);
       bool DeleteObject(int Id);
       bool IsNameDuplicated(Cost model);
       int GetLastMasterCode(int officeId);
    }
}