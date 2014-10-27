using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface ITruckRepository : IRepository<Truck>
    {
       IQueryable<Truck> GetQueryable();
       Truck GetObjectById(int Id);
       Truck CreateObject(Truck model);
       Truck UpdateObject(Truck model);
       Truck SoftDeleteObject(Truck model);
       bool DeleteObject(int Id);
       bool IsNameDuplicated(Truck model);
       int GetLastMasterCode(int officeId);
    }
}