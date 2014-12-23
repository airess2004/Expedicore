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
    public class ShipmentInstructionService : IShipmentInstructionService 
    {  
        private IShipmentInstructionRepository _repository;
        private IShipmentInstructionValidation _validator;

        public ShipmentInstructionService(IShipmentInstructionRepository _shipmentinstructionRepository, IShipmentInstructionValidation _shipmentinstructionValidation)
        {
            _repository = _shipmentinstructionRepository;
            _validator = _shipmentinstructionValidation;
        }

        public IQueryable<ShipmentInstruction> GetQueryable()
        {  
            return _repository.GetQueryable();
        }

        public ShipmentInstruction GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public ShipmentInstruction GetObjectByShipmentOrderId(int Id)
        {
            return _repository.GetObjectByShipmentOrderId(Id);
        }

        public ShipmentInstruction CreateUpdateObject(ShipmentInstruction shipmentinstruction)
        {
            ShipmentInstruction newshipmentinstruction = this.GetObjectByShipmentOrderId(shipmentinstruction.ShipmentOrderId);
            if (newshipmentinstruction == null)
            {
                shipmentinstruction = this.CreateObject(shipmentinstruction);
            }
            else
            {
                shipmentinstruction = this.UpdateObject(shipmentinstruction);
            }
            return shipmentinstruction;
        }

        public ShipmentInstruction CreateObject(ShipmentInstruction shipmentinstruction)
        {
            shipmentinstruction.Errors = new Dictionary<String, String>();
            if (isValid(_validator.VCreateObject(shipmentinstruction,this)))
            {
                shipmentinstruction = _repository.CreateObject(shipmentinstruction);
            }
            return shipmentinstruction;
        }
         
        public ShipmentInstruction UpdateObject(ShipmentInstruction shipmentinstruction)
        {
            if (isValid(_validator.VUpdateObject(shipmentinstruction, this)))
            {
                shipmentinstruction = _repository.UpdateObject(shipmentinstruction);
            }
            return shipmentinstruction;
        }
         
        public ShipmentInstruction SoftDeleteObject(ShipmentInstruction shipmentinstruction)
        {
            shipmentinstruction = _repository.SoftDeleteObject(shipmentinstruction);
            return shipmentinstruction;
        }


        public bool isValid(ShipmentInstruction obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }
    }
}
