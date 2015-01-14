using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IShipmentOrderRoutingRepository : IRepository<ShipmentOrderRouting>
    { 
       IQueryable<ShipmentOrderRouting> GetQueryable();
       ShipmentOrderRouting GetObjectById(int Id);
       ShipmentOrderRouting GetObjectByShipmentOrderId(int Id);
       IList<ShipmentOrderRouting> GetListByShipmentOrderId(int Id);
       ShipmentOrderRouting CreateObject(ShipmentOrderRouting model);
       ShipmentOrderRouting UpdateObject(ShipmentOrderRouting model);
       ShipmentOrderRouting SoftDeleteObject(ShipmentOrderRouting model);
       bool DeleteObject(int Id);
       int GetLastMasterCode(int officeId);
    }
}