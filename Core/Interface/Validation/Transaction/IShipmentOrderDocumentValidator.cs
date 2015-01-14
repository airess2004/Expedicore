﻿using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Validation
{
    public interface IShipmentOrderDocumentValidation
    {
        ShipmentOrderDocument VCreateObject(ShipmentOrderDocument shipmentOrderDocument, IShipmentOrderDocumentService _shipmentOrderDocumentService);
        ShipmentOrderDocument VUpdateObject(ShipmentOrderDocument shipmentOrderDocument, IShipmentOrderDocumentService _shipmentOrderDocumentService);
    }
}
