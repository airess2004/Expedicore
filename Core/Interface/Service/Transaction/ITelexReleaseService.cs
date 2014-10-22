using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface ITelexReleaseService
    {
        IQueryable<TelexRelease> GetQueryable();
        TelexRelease GetObjectById(int Id);
        TelexRelease GetObjectByShipmentOrderId(int Id);
        TelexRelease CreateUpdateObject(TelexRelease telexrelease);
        TelexRelease CreateObject(TelexRelease telexrelease);
        TelexRelease UpdateObject(TelexRelease telexrelease); 
        TelexRelease SoftDeleteObject(TelexRelease telexrelease);
    }
}