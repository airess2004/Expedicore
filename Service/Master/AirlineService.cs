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
    public class AirlineService : IAirlineService 
    {  
        private IAirlineRepository _repository;
        private IAirlineValidation _validator;

        public AirlineService(IAirlineRepository _airlineRepository, IAirlineValidation _airlineValidation)
        {
            _repository = _airlineRepository;
            _validator = _airlineValidation;
        }

        public IQueryable<Airline> GetQueryable()
        {  
            return _repository.GetQueryable();
        }

        public Airline GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }
         
        public Airline CreateObject(Airline airline)
        {
            airline.Errors = new Dictionary<String, String>();
            if (!isValid(_validator.VCreateObject(airline,this)))
            {
                airline.MasterCode = _repository.GetLastMasterCode(airline.OfficeId) + 1;
                airline = _repository.CreateObject(airline);
            }
            return airline;
        }
         
        public Airline UpdateObject(Airline airline)
        {
            if (!isValid(_validator.VUpdateObject(airline, this)))
            {
                airline = _repository.UpdateObject(airline);
            }
            return airline;
        }
         
        public Airline SoftDeleteObject(Airline airline)
        {
            airline = _repository.SoftDeleteObject(airline);
            return airline;
        }

        public bool IsNameDuplicated(Airline airline)
        {
            return _repository.IsNameDuplicated(airline);
        }


        public bool isValid(Airline obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }
    }
}
