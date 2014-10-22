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
    public class ShipmentAdviceValidation : IShipmentAdviceValidation
    {  
        
        public ShipmentAdvice VCreateObject(ShipmentAdvice shipmentAdvice, IShipmentAdviceService _shipmentAdviceService)
        {
            return shipmentAdvice;
        }

        public ShipmentAdvice VUpdateObject(ShipmentAdvice shipmentAdvice, IShipmentAdviceService _shipmentAdviceService)
        { 
            return shipmentAdvice;
        }

        public bool isValid(ShipmentAdvice obj)
        { 
            bool isValid = !obj.Errors.Any();
            return isValid;
        }
    }
}
