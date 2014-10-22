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
    public class ContinentService : IContinentService 
    {  
        private IContinentRepository _repository;
        private IContinentValidation _validator;

        public ContinentService(IContinentRepository _continentRepository, IContinentValidation _continentValidation)
        {
            _repository = _continentRepository;
            _validator = _continentValidation;
        }

        public IQueryable<Continent> GetQueryable()
        {  
            return _repository.GetQueryable();
        }

        public Continent GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }
         
        public Continent CreateObject(Continent continent)
        {
            continent.Errors = new Dictionary<String, String>();
            if (!isValid(_validator.VCreateObject(continent,this)))
            {
                continent.MasterCode = _repository.GetLastMasterCode(continent.OfficeId) + 1;
                continent = _repository.CreateObject(continent);
            }
            return continent;
        }
         
        public Continent UpdateObject(Continent continent)
        {
            if (!isValid(_validator.VUpdateObject(continent, this)))
            {
                continent = _repository.UpdateObject(continent);
            }
            return continent;
        }
         
        public Continent SoftDeleteObject(Continent continent)
        {
            continent = _repository.SoftDeleteObject(continent);
            return continent;
        }

        public bool IsNameDuplicated(Continent continent)
        {
            return _repository.IsNameDuplicated(continent);
        }


        public bool isValid(Continent obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }
    }
}
