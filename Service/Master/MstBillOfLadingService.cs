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
    public class MstBillOfLadingService : IMstBillOfLadingService 
    {  
        private IMstBillOfLadingRepository _repository;
        private IMstBillOfLadingValidation _validator;

        public MstBillOfLadingService(IMstBillOfLadingRepository _costRepository, IMstBillOfLadingValidation _costValidation)
        {
            _repository = _costRepository;
            _validator = _costValidation;
        }

        public IQueryable<MstBillOfLading> GetQueryable()
        {  
            return _repository.GetQueryable();
        }

        public MstBillOfLading GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }
         
        public MstBillOfLading CreateObject(MstBillOfLading cost)
        {
            cost.Errors = new Dictionary<String, String>();
            if (!isValid(_validator.VCreateObject(cost,this)))
            {
                cost.MasterCode = _repository.GetLastMasterCode(cost.OfficeId) + 1;
                cost = _repository.CreateObject(cost);
            }
            return cost;
        }
         
        public MstBillOfLading UpdateObject(MstBillOfLading cost)
        {
            if (!isValid(_validator.VUpdateObject(cost, this)))
            {
                cost = _repository.UpdateObject(cost);
            }
            return cost;
        }
         
        public MstBillOfLading SoftDeleteObject(MstBillOfLading cost)
        {
            cost = _repository.SoftDeleteObject(cost);
            return cost;
        }

        public bool IsNameDuplicated(MstBillOfLading cost)
        {
            return _repository.IsNameDuplicated(cost);
        }


        public bool isValid(MstBillOfLading obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }
    }
}
