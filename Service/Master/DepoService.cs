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
    public class DepoService : IDepoService 
    {  
        private IDepoRepository _repository;
        private IDepoValidation _validator;

        public DepoService(IDepoRepository _depoRepository, IDepoValidation _depoValidation)
        {
            _repository = _depoRepository;
            _validator = _depoValidation;
        }

        public IQueryable<Depo> GetQueryable()
        {  
            return _repository.GetQueryable();
        }

        public Depo GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }
         
        public Depo CreateObject(Depo depo)
        {
            depo.Errors = new Dictionary<String, String>();
            if (isValid(_validator.VCreateObject(depo,this)))
            {
                depo.MasterCode = _repository.GetLastMasterCode(depo.OfficeId) + 1;
                depo = _repository.CreateObject(depo);
            }
            return depo;
        }
         
        public Depo UpdateObject(Depo depo)
        {
            if (isValid(_validator.VUpdateObject(depo, this)))
            {
                depo = _repository.UpdateObject(depo);
            }
            return depo;
        }
         
        public Depo SoftDeleteObject(Depo depo)
        {
            depo = _repository.SoftDeleteObject(depo);
            return depo;
        }

        public bool IsNameDuplicated(Depo depo)
        {
            return _repository.IsNameDuplicated(depo);
        }


        public bool isValid(Depo obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }
    }
}
