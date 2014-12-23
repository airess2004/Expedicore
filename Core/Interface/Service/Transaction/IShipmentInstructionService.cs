using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IShipmentInstructionService
    {
        IQueryable<ShipmentInstruction> GetQueryable();
        ShipmentInstruction GetObjectById(int Id);
        ShipmentInstruction GetObjectByShipmentOrderId(int Id);
        ShipmentInstruction CreateUpdateObject(ShipmentInstruction shipmentinstruction);
        ShipmentInstruction CreateObject(ShipmentInstruction shipmentinstruction);
        ShipmentInstruction UpdateObject(ShipmentInstruction shipmentinstruction); 
        ShipmentInstruction SoftDeleteObject(ShipmentInstruction shipmentinstruction);
    }
}