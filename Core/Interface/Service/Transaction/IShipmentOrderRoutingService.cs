using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IShipmentOrderRoutingService
    {
        IQueryable<ShipmentOrderRouting> GetQueryable();
        ShipmentOrderRouting GetObjectById(int Id);
        ShipmentOrderRouting GetObjectByShipmentOrderId(int Id);
        IList<ShipmentOrderRouting> GetListByShipmentOrderId(int Id);
        ShipmentOrderRouting CreateUpdateObject(ShipmentOrderRouting shipmentorderrouting);
        ShipmentOrderRouting CreateObject(ShipmentOrderRouting shipmentorderrouting);
        ShipmentOrderRouting UpdateObject(ShipmentOrderRouting shipmentorderrouting); 
        ShipmentOrderRouting SoftDeleteObject(ShipmentOrderRouting shipmentorderrouting);
        bool DeleteObject(int Id); 
    }
}