﻿using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Validation
{
    public interface IShipmentAdviceValidation
    {
        ShipmentAdvice VCreateObject(ShipmentAdvice shipmentadvice, IShipmentAdviceService _shipmentadviceService);
        ShipmentAdvice VUpdateObject(ShipmentAdvice shipmentadvice, IShipmentAdviceService _shipmentadviceService);
    }
}
