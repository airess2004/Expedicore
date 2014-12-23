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
    public class VatService : IVatService 
    {  
        private IVatRepository _repository;
        private IVatValidation _validator;

        public VatService(IVatRepository _vatRepository, IVatValidation _vatValidation)
        {
            _repository = _vatRepository;
            _validator = _vatValidation;
        }

        public IQueryable<Vat> GetQueryable()
        {  
            return _repository.GetQueryable();
        }

        public Vat GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }
         
        public Vat CreateObject(Vat vat)
        {
            vat.Errors = new Dictionary<String, String>();
            if (isValid(_validator.VCreateObject(vat,this)))
            {
                vat.MasterCode = _repository.GetLastMasterCode(vat.OfficeId) + 1;
                vat = _repository.CreateObject(vat);
            }
            return vat;
        }
         
        public Vat UpdateObject(Vat vat)
        {
            if (isValid(_validator.VUpdateObject(vat, this)))
            {
                vat = _repository.UpdateObject(vat);
            }
            return vat;
        }
         
        public Vat SoftDeleteObject(Vat vat)
        {
            vat = _repository.SoftDeleteObject(vat);
            return vat;
        }

        public bool IsNameDuplicated(Vat vat)
        {
            return _repository.IsNameDuplicated(vat);
        }


        public bool isValid(Vat obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }
    }
}
