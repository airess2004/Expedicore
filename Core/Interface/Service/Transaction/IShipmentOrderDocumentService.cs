using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IShipmentOrderDocumentService
    {
        IQueryable<ShipmentOrderDocument> GetQueryable();
        ShipmentOrderDocument GetObjectById(int Id);
        ShipmentOrderDocument GetObjectByShipmentOrderId(int Id);
        IList<ShipmentOrderDocument> GetListByShipmentOrderId(int Id);
        ShipmentOrderDocument CreateUpdateObject(ShipmentOrderDocument shipmentOrderDocument);
        ShipmentOrderDocument CreateObject(ShipmentOrderDocument shipmentOrderDocument);
        ShipmentOrderDocument UpdateObject(ShipmentOrderDocument shipmentOrderDocument); 
        ShipmentOrderDocument SoftDeleteObject(ShipmentOrderDocument shipmentOrderDocument);
        bool DeleteObject(int Id); 
    }
}