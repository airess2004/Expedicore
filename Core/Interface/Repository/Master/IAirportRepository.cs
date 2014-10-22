using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IAirportRepository : IRepository<Airport>
    {
       IQueryable<Airport> GetQueryable();
       Airport GetObjectById(int Id);
       Airport CreateObject(Airport model);
       Airport UpdateObject(Airport model);
       Airport SoftDeleteObject(Airport model); 
       bool DeleteObject(int Id);
       bool IsNameDuplicated(Airport model);
       int GetLastMasterCode(int officeId);
    }
}