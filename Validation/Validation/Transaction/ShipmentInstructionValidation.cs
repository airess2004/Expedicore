using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validation.Validation
{ 
    public class ShipmentInstructionValidation : IShipmentInstructionValidation
    {  
        
        public ShipmentInstruction VCreateObject(ShipmentInstruction shipmentInstruction, IShipmentInstructionService _shipmentInstructionService)
        {
            return shipmentInstruction;
        }

        public ShipmentInstruction VUpdateObject(ShipmentInstruction shipmentInstruction, IShipmentInstructionService _shipmentInstructionService)
        { 
            return shipmentInstruction;
        }

        public bool isValid(ShipmentInstruction obj)
        { 
            bool isValid = !obj.Errors.Any();
            return isValid;
        }
    }
}
