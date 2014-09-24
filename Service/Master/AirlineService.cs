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
    public class AirlineService : IAirlineService 
    { 
        private IAirlineRepository _repository;
        private IAirlineValidator _validator;

        public AirlineService(IAirlineRepository _airlineRepository, IAirlineValidator _airlineValidator)
        {
            _repository = _airlineRepository;
            _validator = _airlineValidator;
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
            airline = _repository.CreateObject(airline);
            return airline;
        }

        //public Airline UpdateObject(Airline contact, IContactGroupService _contactGroupService)
        //{
        //    return (contact = _validator.ValidUpdateObject(contact, this, _contactGroupService) ? _repository.UpdateObject(contact) : contact);
        //}

    }
}
