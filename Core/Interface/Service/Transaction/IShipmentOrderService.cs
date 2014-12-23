using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IShipmentOrderService
    {
        IQueryable<ShipmentOrder> GetQueryable();
        ShipmentOrder GetObjectById(int Id);
        ShipmentOrder CreateObject(ShipmentOrder shipmentorder, IOfficeService _officeService);
        ShipmentOrder UpdateObject(ShipmentOrder shipmentorder);
        ShipmentOrder SoftDeleteObject(ShipmentOrder shipmentorder);
    }
}