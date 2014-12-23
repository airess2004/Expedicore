using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IShipmentInstructionRepository : IRepository<ShipmentInstruction>
    { 
       IQueryable<ShipmentInstruction> GetQueryable();
       ShipmentInstruction GetObjectById(int Id);
       ShipmentInstruction GetObjectByShipmentOrderId(int Id);
       ShipmentInstruction CreateObject(ShipmentInstruction model);
       ShipmentInstruction UpdateObject(ShipmentInstruction model);
       ShipmentInstruction SoftDeleteObject(ShipmentInstruction model);
       bool DeleteObject(int Id);
    }
}