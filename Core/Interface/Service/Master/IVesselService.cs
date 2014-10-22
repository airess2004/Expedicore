using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IVesselService
    {
        IQueryable<Vessel> GetQueryable();
        Vessel GetObjectById(int Id);
        Vessel CreateObject(Vessel vessel);
        Vessel UpdateObject(Vessel vessel);
        Vessel SoftDeleteObject(Vessel vessel);
        bool IsNameDuplicated(Vessel vessel);
    }
}