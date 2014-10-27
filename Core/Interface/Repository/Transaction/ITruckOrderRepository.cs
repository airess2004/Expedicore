using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface ITruckOrderRepository : IRepository<TruckOrder>
    { 
       IQueryable<TruckOrder> GetQueryable();
       TruckOrder GetObjectById(int Id);
       TruckOrder CreateObject(TruckOrder model);
       TruckOrder UpdateObject(TruckOrder model);
       TruckOrder SoftDeleteObject(TruckOrder model);
       int GetLastMasterCode(int officeId);
       bool DeleteObject(int Id);
    }
}