using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Master
{
    public class ShipmentOrderRoutingService : IShipmentOrderRoutingService 
    {  
        private IShipmentOrderRoutingRepository _repository;
        private IShipmentOrderRoutingValidation _validator;

        public ShipmentOrderRoutingService(IShipmentOrderRoutingRepository _shipmentorderroutingRepository, IShipmentOrderRoutingValidation _shipmentorderroutingValidation)
        {
            _repository = _shipmentorderroutingRepository;
            _validator = _shipmentorderroutingValidation;
        }

        public IQueryable<ShipmentOrderRouting> GetQueryable()
        {  
            return _repository.GetQueryable();
        }

        public ShipmentOrderRouting GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public ShipmentOrderRouting GetObjectByShipmentOrderId(int Id)
        {
            return _repository.GetObjectByShipmentOrderId(Id);
        }

        public IList<ShipmentOrderRouting> GetListByShipmentOrderId(int Id)
        { 
            return _repository.GetListByShipmentOrderId(Id);
        }


        public ShipmentOrderRouting CreateUpdateObject(ShipmentOrderRouting shipmentorderrouting)
        {
            ShipmentOrderRouting newshipmentorderrouting = this.GetObjectByShipmentOrderId(shipmentorderrouting.ShipmentOrderId);
            if (newshipmentorderrouting == null)
            {
                shipmentorderrouting = this.CreateObject(shipmentorderrouting);
            }
            else
            {
                shipmentorderrouting = this.UpdateObject(shipmentorderrouting);
            }
            return shipmentorderrouting;
        }

        public ShipmentOrderRouting CreateObject(ShipmentOrderRouting shipmentorderrouting)
        {
            shipmentorderrouting.Errors = new Dictionary<String, String>();
            if (!isValid(_validator.VCreateObject(shipmentorderrouting,this)))
            {
                shipmentorderrouting = _repository.CreateObject(shipmentorderrouting);
            }
            return shipmentorderrouting;
        }
         
        public ShipmentOrderRouting UpdateObject(ShipmentOrderRouting shipmentorderrouting)
        {
            if (!isValid(_validator.VUpdateObject(shipmentorderrouting, this)))
            {
                shipmentorderrouting = _repository.UpdateObject(shipmentorderrouting);
            }
            return shipmentorderrouting;
        }
         
        public ShipmentOrderRouting SoftDeleteObject(ShipmentOrderRouting shipmentorderrouting)
        {
            shipmentorderrouting = _repository.SoftDeleteObject(shipmentorderrouting);
            return shipmentorderrouting;
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }
        public bool isValid(ShipmentOrderRouting obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }
    }
}
