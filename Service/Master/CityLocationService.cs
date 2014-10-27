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
    public class CityLocationService : ICityLocationService 
    {  
        private ICityLocationRepository _repository;
        private ICityLocationValidation _validator;

        public CityLocationService(ICityLocationRepository _citylocationRepository, ICityLocationValidation _citylocationValidation)
        {
            _repository = _citylocationRepository;
            _validator = _citylocationValidation;
        }

        public IQueryable<CityLocation> GetQueryable()
        {  
            return _repository.GetQueryable();
        }

        public CityLocation GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }
         
        public CityLocation CreateObject(CityLocation citylocation,ICountryLocationService _countrylocationService)
        {
            citylocation.Errors = new Dictionary<String, String>();
            if (!isValid(_validator.VCreateObject(citylocation,this,_countrylocationService)))
            {
                citylocation.MasterCode = _repository.GetLastMasterCode(citylocation.OfficeId) + 1;
                citylocation = _repository.CreateObject(citylocation);
            }
            return citylocation;
        }

        public CityLocation UpdateObject(CityLocation citylocation, ICountryLocationService _countrylocationService)
        {
            if (!isValid(_validator.VUpdateObject(citylocation, this,_countrylocationService)))
            {
                citylocation = _repository.UpdateObject(citylocation);
            }
            return citylocation;
        }
         
        public CityLocation SoftDeleteObject(CityLocation citylocation)
        {
            citylocation = _repository.SoftDeleteObject(citylocation);
            return citylocation;
        }

        public bool IsNameDuplicated(CityLocation citylocation)
        {
            return _repository.IsNameDuplicated(citylocation);
        }


        public bool isValid(CityLocation obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }
    }
}
