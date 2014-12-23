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
    public class BillOfLadingService : IBillOfLadingService 
    {  
        private IBillOfLadingRepository _repository;
        private IBillOfLadingValidation _validator;

        public BillOfLadingService(IBillOfLadingRepository _billofladingRepository, IBillOfLadingValidation _billofladingValidation)
        {
            _repository = _billofladingRepository;
            _validator = _billofladingValidation;
        }

        public IQueryable<BillOfLading> GetQueryable()
        {  
            return _repository.GetQueryable();
        }

        public BillOfLading GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public BillOfLading GetObjectByShipmentOrderId(int Id)
        {
            return _repository.GetObjectByShipmentOrderId(Id);
        }

        public BillOfLading CreateUpdateObject(BillOfLading billoflading)
        {
            BillOfLading newbilloflading = this.GetObjectByShipmentOrderId(billoflading.ShipmentOrderId);
            if (newbilloflading == null)
            {
                billoflading = this.CreateObject(billoflading);
            }
            else
            {
                billoflading = this.UpdateObject(billoflading);
            }
            return billoflading;
        }

        public BillOfLading CreateObject(BillOfLading billoflading)
        {
            billoflading.Errors = new Dictionary<String, String>();
            if (isValid(_validator.VCreateObject(billoflading,this)))
            {
                billoflading = _repository.CreateObject(billoflading);
            }
            return billoflading;
        }
         
        public BillOfLading UpdateObject(BillOfLading billoflading)
        {
            if (isValid(_validator.VUpdateObject(billoflading, this)))
            {
                billoflading = _repository.UpdateObject(billoflading);
            }
            return billoflading;
        }
         
        public BillOfLading SoftDeleteObject(BillOfLading billoflading)
        {
            billoflading = _repository.SoftDeleteObject(billoflading);
            return billoflading;
        }

        public bool isValid(BillOfLading obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }
    }
}
