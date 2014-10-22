using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IVesselRepository : IRepository<Vessel>
    {
       IQueryable<Vessel> GetQueryable();
       Vessel GetObjectById(int Id);
       Vessel CreateObject(Vessel model);
       Vessel UpdateObject(Vessel model);
       Vessel SoftDeleteObject(Vessel model);
       int GetLastMasterCode(int officeId);
       bool DeleteObject(int Id);
       bool IsNameDuplicated(Vessel model);
    }
}