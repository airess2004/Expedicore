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
    public class ShipmentOrderValidation : IShipmentOrderValidation
    {

        public ShipmentOrder VAgent(ShipmentOrder shipmentorder, IShipmentOrderService _shipmentorderService, IContactService _contactService)
        {
            if (shipmentorder.AgentId == null)
            {
                shipmentorder.Errors.Add("Generic", "Invalid Agent");
            }
            else if (_contactService.GetObjectById(shipmentorder.AgentId.Value) == null)
                {
                    shipmentorder.Errors.Add("Generic", "Invalid Agent");
                }
            
            return shipmentorder;
        }

        public ShipmentOrder VConsignee(ShipmentOrder shipmentorder, IShipmentOrderService _shipmentorderService, IContactService _contactService)
        {
            if (shipmentorder.ConsigneeId == null)
            {
                shipmentorder.Errors.Add("Generic", "Invalid Consignee");
            }
            else if (_contactService.GetObjectById(shipmentorder.ConsigneeId.Value) == null)
                {
                    shipmentorder.Errors.Add("Generic", "Invalid Consignee");
                }
            
            return shipmentorder;
        }

        public ShipmentOrder VShipper(ShipmentOrder shipmentorder, IShipmentOrderService _shipmentorderService, IContactService _contactService)
        {
            if (shipmentorder.ShipperId == null)
            {
                shipmentorder.Errors.Add("Generic", "Invalid Shipper");
            }
            else if (_contactService.GetObjectById(shipmentorder.ShipperId.Value) == null)
                {
                    shipmentorder.Errors.Add("Generic", "Invalid Shipper");
                }
            
            return shipmentorder;
        }

        public ShipmentOrder VTranshipment(ShipmentOrder shipmentorder, IShipmentOrderService _shipmentorderService, IContactService _contactService)
        {
            if (shipmentorder.TranshipmentId == null)
            {
                shipmentorder.Errors.Add("Generic", "Invalid Transhipment");
            }
            else if (_contactService.GetObjectById(shipmentorder.TranshipmentId.Value) == null)
                {
                    shipmentorder.Errors.Add("Generic", "Invalid Transhipment");
                }
            
            return shipmentorder;
        }

        public ShipmentOrder VDelivery(ShipmentOrder shipmentorder, IShipmentOrderService _shipmentorderService, IContactService _contactService)
        {
            if (shipmentorder.DeliveryId == null)
            {
                shipmentorder.Errors.Add("Generic", "Invalid Delivery");
            }
            else if (_contactService.GetObjectById(shipmentorder.DeliveryId.Value) == null)
                {
                    shipmentorder.Errors.Add("Generic", "Invalid Delivery");
                }
            return shipmentorder;
        }

        public ShipmentOrder VOBL(ShipmentOrder shipmentorder, IShipmentOrderService _shipmentorderService, IContactService _contactService,ICityLocationService _cityService)
        {
            if (shipmentorder.OBLStatus != "P")
            {
                if (shipmentorder.OBLCollectId == null)
                {
                    shipmentorder.Errors.Add("Generic", "Invalid Freight Collect At");
                }
                else if (_cityService.GetObjectById(shipmentorder.OBLCollectId.Value) == null)
                {
                    shipmentorder.Errors.Add("Generic", "Invalid Freight Collect At");
                }
            }
            return shipmentorder;
        }


        public ShipmentOrder VCreateObject(ShipmentOrder shipmentorder, IShipmentOrderService _shipmentorderService)
        {
            return shipmentorder;
        }

        public ShipmentOrder VUpdateObject(ShipmentOrder shipmentorder, IShipmentOrderService _shipmentorderService, IContactService _contactService)
        {
            VAgent(shipmentorder, _shipmentorderService, _contactService);
            if (!isValid(shipmentorder)) { return shipmentorder; }
            VDelivery(shipmentorder, _shipmentorderService, _contactService);
            if (!isValid(shipmentorder)) { return shipmentorder; }
            VTranshipment(shipmentorder, _shipmentorderService, _contactService);
            if (!isValid(shipmentorder)) { return shipmentorder; }
            VShipper(shipmentorder, _shipmentorderService, _contactService);
            if (!isValid(shipmentorder)) { return shipmentorder; }
            VConsignee(shipmentorder, _shipmentorderService, _contactService);
            if (!isValid(shipmentorder)) { return shipmentorder; }
            return shipmentorder;
        }

        public ShipmentOrder VUpdateObjectSeaExport(ShipmentOrder shipmentorder, IShipmentOrderService _shipmentorderService, IContactService _contactService)
        { 
            VAgent(shipmentorder, _shipmentorderService, _contactService);
            if (!isValid(shipmentorder)) { return shipmentorder; }
            VDelivery(shipmentorder, _shipmentorderService, _contactService);
            if (!isValid(shipmentorder)) { return shipmentorder; }
            VTranshipment(shipmentorder, _shipmentorderService, _contactService);
            if (!isValid(shipmentorder)) { return shipmentorder; }
            VShipper(shipmentorder, _shipmentorderService, _contactService);
            if (!isValid(shipmentorder)) { return shipmentorder; }
            VConsignee(shipmentorder, _shipmentorderService, _contactService);
            if (!isValid(shipmentorder)) { return shipmentorder; }
            return shipmentorder;
        }

        public bool isValid(ShipmentOrder obj)
        { 
            bool isValid = !obj.Errors.Any();
            return isValid;
        }
    }
}
