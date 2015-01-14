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
    public class CostService : ICostService 
    {  
        private ICostRepository _repository;
        private ICostValidation _validator;

        public CostService(ICostRepository _costRepository, ICostValidation _costValidation)
        {
            _repository = _costRepository;
            _validator = _costValidation;
        }

        public IQueryable<Cost> GetQueryable()
        {  
            return _repository.GetQueryable();
        }

        public Cost GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }
         
        public Cost CreateObject(Cost cost)
        {
            cost.Errors = new Dictionary<String, String>();
            if (isValid(_validator.VCreateObject(cost,this)))
            {
                cost.MasterCode = _repository.GetLastMasterCode(cost.OfficeId) + 1;
                cost = _repository.CreateObject(cost);
            }
            return cost;
        }
         
        public Cost UpdateObject(Cost cost)
        {
            if (isValid(_validator.VUpdateObject(cost, this)))
            {
                cost = _repository.UpdateObject(cost);
            }
            return cost;
        }
         
        public Cost SoftDeleteObject(Cost cost)
        {
            cost = _repository.SoftDeleteObject(cost);
            return cost;
        }

        public bool IsNameDuplicated(Cost cost)
        {
            return _repository.IsNameDuplicated(cost);
        }


        public bool isValid(Cost obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }
    }
}
