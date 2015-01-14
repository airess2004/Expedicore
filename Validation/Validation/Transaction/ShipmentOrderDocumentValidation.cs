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
    public class ShipmentOrderDocumentValidation : IShipmentOrderDocumentValidation
    {  
        
        public ShipmentOrderDocument VCreateObject(ShipmentOrderDocument shipmentOrderDocument, IShipmentOrderDocumentService _shipmentOrderDocumentService)
        {
            return shipmentOrderDocument;
        }

        public ShipmentOrderDocument VUpdateObject(ShipmentOrderDocument shipmentOrderDocument, IShipmentOrderDocumentService _shipmentOrderDocumentService)
        { 
            return shipmentOrderDocument;
        }

        public bool isValid(ShipmentOrderDocument obj)
        { 
            bool isValid = !obj.Errors.Any();
            return isValid;
        }
    }
}
