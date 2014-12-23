using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IShipmentAdviceRepository : IRepository<ShipmentAdvice>
    { 
       IQueryable<ShipmentAdvice> GetQueryable();
       ShipmentAdvice GetObjectById(int Id);
       ShipmentAdvice GetObjectByShipmentOrderId(int Id);
       ShipmentAdvice CreateObject(ShipmentAdvice model);
       ShipmentAdvice UpdateObject(ShipmentAdvice model);
       ShipmentAdvice SoftDeleteObject(ShipmentAdvice model);
       bool DeleteObject(int Id);
    }
}