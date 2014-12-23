using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Validation
{
    public interface IShipmentInstructionValidation
    {
        ShipmentInstruction VCreateObject(ShipmentInstruction shipmentinstruction, IShipmentInstructionService _shipmentinstructionService);
        ShipmentInstruction VUpdateObject(ShipmentInstruction shipmentinstruction, IShipmentInstructionService _shipmentinstructionService);
    }
}
