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
    public class CountryLocationService : ICountryLocationService 
    {  
        private ICountryLocationRepository _repository;
        private ICountryLocationValidation _validator;

        public CountryLocationService(ICountryLocationRepository _countrylocationRepository, ICountryLocationValidation _countrylocationValidation)
        {
            _repository = _countrylocationRepository;
            _validator = _countrylocationValidation;
        }

        public IQueryable<CountryLocation> GetQueryable()
        {  
            return _repository.GetQueryable();
        }

        public CountryLocation GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }
         
        public CountryLocation CreateObject(CountryLocation countrylocation,IContinentService _continentService)
        {
            countrylocation.Errors = new Dictionary<String, String>();
            if (isValid(_validator.VCreateObject(countrylocation,this,_continentService)))
            {
                countrylocation.MasterCode = _repository.GetLastMasterCode(countrylocation.OfficeId) + 1;
                countrylocation = _repository.CreateObject(countrylocation);
            }
            return countrylocation;
        }

        public CountryLocation UpdateObject(CountryLocation countrylocation, IContinentService _continentService)
        {
            if (isValid(_validator.VUpdateObject(countrylocation, this,_continentService)))
            {
                countrylocation = _repository.UpdateObject(countrylocation);
            }
            return countrylocation;
        }
         
        public CountryLocation SoftDeleteObject(CountryLocation countrylocation)
        {
            countrylocation = _repository.SoftDeleteObject(countrylocation);
            return countrylocation;
        }

        public bool IsNameDuplicated(CountryLocation countrylocation)
        {
            return _repository.IsNameDuplicated(countrylocation);
        }


        public bool isValid(CountryLocation obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }
    }
}
