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
    public class AirportService : IAirportService 
    {  
        private IAirportRepository _repository;
        private IAirportValidation _validator;

        public AirportService(IAirportRepository _airportRepository, IAirportValidation _airportValidation)
        {
            _repository = _airportRepository;
            _validator = _airportValidation;
        }

        public IQueryable<Airport> GetQueryable()
        {  
            return _repository.GetQueryable();
        }

        public Airport GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        
        public Airport CreateObject(Airport airport, ICityLocationService _citylocationservice)
        {
            airport.Errors = new Dictionary<String, String>();
            if (isValid(_validator.VCreateObject(airport, this, _citylocationservice)))
            {
                airport.MasterCode = _repository.GetLastMasterCode(airport.OfficeId) + 1;
                airport = _repository.CreateObject(airport);
            }
            return airport;
        }
         
        public Airport UpdateObject(Airport airport, ICityLocationService _citylocationservice)
        {
            if (isValid(_validator.VUpdateObject(airport, this, _citylocationservice)))
            {
                airport = _repository.UpdateObject(airport);
            }
            return airport;
        }
         
        public Airport SoftDeleteObject(Airport airport)
        {
            airport = _repository.SoftDeleteObject(airport);
            return airport;
        }

        public bool IsNameDuplicated(Airport airport)
        {
            return _repository.IsNameDuplicated(airport);
        }


        public bool isValid(Airport obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }
    }
}
