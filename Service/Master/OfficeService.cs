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
    public class OfficeService : IOfficeService 
    {  
        private IOfficeRepository _repository;
        private IOfficeValidation _validator;

        public OfficeService(IOfficeRepository _officeRepository, IOfficeValidation _officeValidation)
        {
            _repository = _officeRepository;
            _validator = _officeValidation;
        }

        public IQueryable<Office> GetQueryable()
        {  
            return _repository.GetQueryable();
        }

        public Office GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }
         
        public Office CreateObject(Office office)
        {
            office.Errors = new Dictionary<String, String>();
            if (!isValid(_validator.VCreateObject(office,this)))
            {
                office = _repository.CreateObject(office);
            }
            return office;
        }
         
        public Office UpdateObject(Office office)
        {
            if (!isValid(_validator.VUpdateObject(office, this)))
            {
                office = _repository.UpdateObject(office);
            }
            return office;
        }
         
        public Office SoftDeleteObject(Office office)
        {
            office = _repository.SoftDeleteObject(office);
            return office;
        }

        public bool IsNameDuplicated(Office office)
        {
            return _repository.IsNameDuplicated(office);
        }


        public bool isValid(Office obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }
    }
}
