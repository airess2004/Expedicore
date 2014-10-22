using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IPortRepository : IRepository<Port>
    {
       IQueryable<Port> GetQueryable();
       Port GetObjectById(int Id);
       Port CreateObject(Port model);
       Port UpdateObject(Port model);
       Port SoftDeleteObject(Port model);
       bool DeleteObject(int Id);
       bool IsNameDuplicated(Port model);
       int GetLastMasterCode(int officeId);
    }
}