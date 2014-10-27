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
    public class TruckOrderService : ITruckOrderService 
    {  
        private ITruckOrderRepository _repository;
        private ITruckOrderValidation _validator;

        public TruckOrderService(ITruckOrderRepository _truckOrderRepository, ITruckOrderValidation _truckOrderValidation)
        {
            _repository = _truckOrderRepository;
            _validator = _truckOrderValidation;
        }

        public IQueryable<TruckOrder> GetQueryable()
        {  
            return _repository.GetQueryable();
        }

        public TruckOrder GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }
         
        public TruckOrder CreateObject(TruckOrder truckOrder)
        {
            truckOrder.Errors = new Dictionary<String, String>();
            if (isValid(_validator.VCreateObject(truckOrder,this)))
            {
                truckOrder.MasterCode = _repository.GetLastMasterCode(truckOrder.OfficeId) + 1;
                truckOrder = _repository.CreateObject(truckOrder);
            }
            return truckOrder;
        }
         
        public TruckOrder UpdateObject(TruckOrder truckOrder)
        {
            if (isValid(_validator.VUpdateObject(truckOrder, this)))
            {
                truckOrder = _repository.UpdateObject(truckOrder);
            }
            return truckOrder;
        }
         
        public TruckOrder SoftDeleteObject(TruckOrder truckOrder)
        {
            truckOrder = _repository.SoftDeleteObject(truckOrder);
            return truckOrder;
        }


        public bool isValid(TruckOrder obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }
    }
}
