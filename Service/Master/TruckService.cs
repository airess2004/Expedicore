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
    public class TruckService : ITruckService 
    {  
        private ITruckRepository _repository;
        private ITruckValidation _validator;

        public TruckService(ITruckRepository _truckRepository, ITruckValidation _truckValidation)
        {
            _repository = _truckRepository;
            _validator = _truckValidation;
        }

        public IQueryable<Truck> GetQueryable()
        {  
            return _repository.GetQueryable();
        }

        public Truck GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }
         
        public Truck CreateObject(Truck truck)
        {
            truck.Errors = new Dictionary<String, String>();
            if (isValid(_validator.VCreateObject(truck,this)))
            {
                truck.MasterCode = _repository.GetLastMasterCode(truck.OfficeId) + 1;
                truck = _repository.CreateObject(truck);
            }
            return truck;
        }
         
        public Truck UpdateObject(Truck truck)
        {
            if (isValid(_validator.VUpdateObject(truck, this)))
            {
                truck = _repository.UpdateObject(truck);
            }
            return truck;
        }
         
        public Truck SoftDeleteObject(Truck truck)
        {
            truck = _repository.SoftDeleteObject(truck);
            return truck;
        }

        public bool IsNameDuplicated(Truck truck)
        {
            return _repository.IsNameDuplicated(truck);
        }


        public bool isValid(Truck obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }
    }
}
