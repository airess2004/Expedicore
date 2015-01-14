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
    public class SeaContainerValidation : ISeaContainerValidation
    {
        public SeaContainer VContainerNo(SeaContainer seaContainer, ISeaContainerService _seaContainerService)
        {
            SeaContainer sc = new SeaContainer();
            if (seaContainer.Id == 0)
            {
               sc  = _seaContainerService.GetQueryable().Where(x => x.ShipmentOrderId == seaContainer.ShipmentOrderId
                                                                            && x.ContainerNo.ToUpper() == seaContainer.ContainerNo.ToUpper()).FirstOrDefault();
            }
            else
            {
                sc = _seaContainerService.GetQueryable().Where(x => x.ShipmentOrderId == seaContainer.ShipmentOrderId
                                                                            && x.ContainerNo.ToUpper() == seaContainer.ContainerNo.ToUpper()
                                                                            && x.Id != seaContainer.Id).FirstOrDefault();
            }
            if (sc != null)
            {
                seaContainer.Errors.Add("Generic", "ContainerNo Already Exist");
            }
            return seaContainer;
        }

        public SeaContainer VShipmentOrder(SeaContainer seaContainer, IShipmentOrderService _shipmentOrderService)
        {
            ShipmentOrder shipmentOrder = _shipmentOrderService.GetObjectById(seaContainer.ShipmentOrderId);
            if (shipmentOrder == null)
            {
                seaContainer.Errors.Add("Generic", "Invalid ShipmentOrder");
            }
            else
            {
                if (shipmentOrder.OfficeId != seaContainer.OfficeId)
                {
                    seaContainer.Errors.Add("Generic", "Invalid ShipmentOrder");
                }
            }
            return seaContainer;
        }


        public SeaContainer VCreateObject(SeaContainer seacontainer, ISeaContainerService _seaContainerService,IShipmentOrderService _shipmentOrderService)
        {
            VContainerNo(seacontainer, _seaContainerService);
            if (!isValid(seacontainer)) { return seacontainer; } 
            VShipmentOrder(seacontainer,_shipmentOrderService);
            if (!isValid(seacontainer)) { return seacontainer; }

            return seacontainer;
        }

        public SeaContainer VUpdateObject(SeaContainer seacontainer, ISeaContainerService _seacontainerService)
        { 
            return seacontainer;
        }

        public bool isValid(SeaContainer obj)
        { 
            bool isValid = !obj.Errors.Any();
            return isValid;
        }
    }
}
