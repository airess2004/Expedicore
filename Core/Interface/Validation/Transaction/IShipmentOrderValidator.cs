using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Validation
{
    public interface IShipmentOrderValidation
    {
        ShipmentOrder VCreateObject(ShipmentOrder shipmentorder, IShipmentOrderService _shipmentorderService);
        ShipmentOrder VUpdateObject(ShipmentOrder shipmentorder, IShipmentOrderService _shipmentorderService, IContactService _contactService);
        ShipmentOrder VUpdateObjectSeaExport(ShipmentOrder shipmentorder, IShipmentOrderService _shipmentorderService, IContactService _contactService);
    }
}
