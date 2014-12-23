using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IShipmentAdviceService
    {
        IQueryable<ShipmentAdvice> GetQueryable();
        ShipmentAdvice GetObjectById(int Id);
        ShipmentAdvice GetObjectByShipmentOrderId(int Id);
        ShipmentAdvice CreateUpdateObject(ShipmentAdvice shipmentadvice);
        ShipmentAdvice CreateObject(ShipmentAdvice shipmentadvice);
        ShipmentAdvice UpdateObject(ShipmentAdvice shipmentadvice); 
        ShipmentAdvice SoftDeleteObject(ShipmentAdvice shipmentadvice);
    }
}