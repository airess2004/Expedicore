using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IShipmentOrderRepository : IRepository<ShipmentOrder>
    { 
       IQueryable<ShipmentOrder> GetQueryable();
       ShipmentOrder GetObjectById(int Id);
       ShipmentOrder CreateObject(ShipmentOrder model);
       ShipmentOrder UpdateObject(ShipmentOrder model);
       ShipmentOrder SoftDeleteObject(ShipmentOrder model);
       bool DeleteObject(int Id);
       int GetLastMasterCode(int officeId);
       int GetLastJobNumber(int officeId, int jobId);
       int GetLastSubJobNumber(int officeId, int jobId, int subJobNumber);
    }
}