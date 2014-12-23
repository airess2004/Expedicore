using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Validation
{
    public interface IShipmentOrderRoutingValidation
    {
        ShipmentOrderRouting VCreateObject(ShipmentOrderRouting shipmentorderrouting, IShipmentOrderRoutingService _shipmentorderroutingService);
        ShipmentOrderRouting VUpdateObject(ShipmentOrderRouting shipmentorderrouting, IShipmentOrderRoutingService _shipmentorderroutingService);
    }
}
