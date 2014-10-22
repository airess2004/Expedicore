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
    public class ShipmentAdviceService : IShipmentAdviceService 
    {  
        private IShipmentAdviceRepository _repository;
        private IShipmentAdviceValidation _validator;

        public ShipmentAdviceService(IShipmentAdviceRepository _shipmentadviceRepository, IShipmentAdviceValidation _shipmentadviceValidation)
        {
            _repository = _shipmentadviceRepository;
            _validator = _shipmentadviceValidation;
        }

        public IQueryable<ShipmentAdvice> GetQueryable()
        {  
            return _repository.GetQueryable();
        }

        public ShipmentAdvice GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public ShipmentAdvice GetObjectByShipmentOrderId(int Id)
        {
            return _repository.GetObjectByShipmentOrderId(Id);
        }

        public ShipmentAdvice CreateUpdateObject(ShipmentAdvice shipmentadvice)
        {
            ShipmentAdvice newshipmentadvice = this.GetObjectByShipmentOrderId(shipmentadvice.ShipmentOrderId);
            if (newshipmentadvice == null)
            {
                shipmentadvice = this.CreateObject(shipmentadvice);
            }
            else
            {
                shipmentadvice = this.UpdateObject(shipmentadvice);
            }
            return shipmentadvice;
        }

        public ShipmentAdvice CreateObject(ShipmentAdvice shipmentadvice)
        {
            shipmentadvice.Errors = new Dictionary<String, String>();
            if (!isValid(_validator.VCreateObject(shipmentadvice,this)))
            {
                shipmentadvice = _repository.CreateObject(shipmentadvice);
            }
            return shipmentadvice;
        }
         
        public ShipmentAdvice UpdateObject(ShipmentAdvice shipmentadvice)
        {
            if (!isValid(_validator.VUpdateObject(shipmentadvice, this)))
            {
                shipmentadvice = _repository.UpdateObject(shipmentadvice);
            }
            return shipmentadvice;
        }
         
        public ShipmentAdvice SoftDeleteObject(ShipmentAdvice shipmentadvice)
        {
            shipmentadvice = _repository.SoftDeleteObject(shipmentadvice);
            return shipmentadvice;
        }


        public bool isValid(ShipmentAdvice obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }
    }
}
