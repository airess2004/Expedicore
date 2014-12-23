using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class SeaContainerService : ISeaContainerService 
    {  
        private ISeaContainerRepository _repository;
        private ISeaContainerValidation _validator;

        public SeaContainerService(ISeaContainerRepository _seacontainerRepository, ISeaContainerValidation _seacontainerValidation)
        {
            _repository = _seacontainerRepository;
            _validator = _seacontainerValidation;   
        }

        public IQueryable<SeaContainer> GetQueryable()
        {  
            return _repository.GetQueryable();
        }

        public SeaContainer GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public SeaContainer GetObjectByShipmentOrderId(int Id)
        {
            return _repository.GetObjectByShipmentOrderId(Id);
        }

        public SeaContainer CreateUpdateObject(SeaContainer seacontainer)
        {
            SeaContainer newseacontainer = this.GetObjectByShipmentOrderId(seacontainer.ShipmentOrderId);

            if (newseacontainer == null)
            {
                seacontainer = this.CreateObject(seacontainer);
            }
            else
            {
                seacontainer = this.UpdateObject(seacontainer);
            }
            return seacontainer;
        }

        public SeaContainer CreateObject(SeaContainer seacontainer)
        {
            seacontainer.Errors = new Dictionary<String, String>();
            if (isValid(_validator.VCreateObject(seacontainer,this)))
            {
                seacontainer = _repository.CreateObject(seacontainer);
            }
            return seacontainer;
        }
         
        public SeaContainer UpdateObject(SeaContainer seacontainer)
        {
            if (isValid(_validator.VUpdateObject(seacontainer, this)))
            {
                seacontainer = _repository.UpdateObject(seacontainer);
            }
            return seacontainer;
        }

        public SeaContainer UpdateContainerInfo(SeaContainer seacontainer, IShipmentOrderService _shipmentorderService)
        {
            ShipmentOrder shipment = _shipmentorderService.GetObjectById(seacontainer.ShipmentOrderId);
            if (shipment != null)
            {
                if (shipment.SubJobNumber > 0)
                {
                    IList<ShipmentOrder> shipmentList = _shipmentorderService.GetQueryable().Where(x => x.JobId == shipment.JobId
                                                         && x.OfficeId == shipment.OfficeId && x.JobNumber == shipment.JobNumber).ToList();
                    ShipmentOrder mainshipment = shipmentList.Where(x => x.SubJobNumber == 0).FirstOrDefault();
                    IList<SeaContainer> containerListMainShipment = GetQueryable().Where(x => x.ShipmentOrderId == mainshipment.Id).ToList();

                    foreach (var clms in containerListMainShipment)
                    {
                        decimal cbm = 0;
                        decimal grossWeight = 0;
                        decimal netWeight = 0;

                        // Get Total CBM, GrossWeight, NetWeight
                        foreach (var item in shipmentList)
                        {
                            if (item.SubJobNumber > 0)
                            {
                                var container = GetQueryable().Where(x => x.ShipmentOrderId == item.Id && x.ContainerNo == clms.ContainerNo).FirstOrDefault();
                                if (container != null)
                                {
                                    var subContainer = GetObjectById(container.Id);
                                    cbm += subContainer.CBM.HasValue ? subContainer.CBM.Value : 0;
                                    netWeight += subContainer.NetWeight.HasValue ? subContainer.NetWeight.Value : 0;
                                    grossWeight += subContainer.GrossWeight.HasValue ? subContainer.GrossWeight.Value : 0;
                                }
                            }
                        }

                        // Update Main Container
                        var mainContainer = GetObjectById(clms.Id);
                        if (mainContainer != null)
                        {
                            mainContainer.CBM = cbm;
                            mainContainer.NetWeight = netWeight;
                            mainContainer.GrossWeight = grossWeight;

                            UpdateObject(mainContainer);
                        }
                    }
                }
            }
            return seacontainer;
        }
        public SeaContainer SoftDeleteObject(SeaContainer seacontainer)
        {
            seacontainer = _repository.SoftDeleteObject(seacontainer);
            return seacontainer;
        }


        public bool isValid(SeaContainer obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }
    }
}
