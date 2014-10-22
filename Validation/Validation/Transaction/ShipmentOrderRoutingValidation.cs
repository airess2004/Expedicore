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
    public class ShipmentOrderRoutingValidation : IShipmentOrderRoutingValidation
    {  
        
        public ShipmentOrderRouting VCreateObject(ShipmentOrderRouting shipmentorderrouting, IShipmentOrderRoutingService _shipmentorderroutingService)
        {
            return shipmentorderrouting;
        }

        public ShipmentOrderRouting VUpdateObject(ShipmentOrderRouting shipmentorderrouting, IShipmentOrderRoutingService _shipmentorderroutingService)
        { 
            return shipmentorderrouting;
        }

        public bool isValid(ShipmentOrderRouting obj)
        { 
            bool isValid = !obj.Errors.Any();
            return isValid;
        }
    }
}
