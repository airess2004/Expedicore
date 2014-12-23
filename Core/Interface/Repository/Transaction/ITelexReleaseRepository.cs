using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface ITelexReleaseRepository : IRepository<TelexRelease>
    { 
       IQueryable<TelexRelease> GetQueryable();
       TelexRelease GetObjectById(int Id);
       TelexRelease GetObjectByShipmentOrderId(int Id);
       TelexRelease CreateObject(TelexRelease model);
       TelexRelease UpdateObject(TelexRelease model);
       TelexRelease SoftDeleteObject(TelexRelease model);
       bool DeleteObject(int Id);
    }
}