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
    public class ContactTypeService : IContactTypeService 
    {  
        private IContactTypeRepository _repository;
        private IContactTypeValidation _validator;

        public ContactTypeService(IContactTypeRepository _contacttypeRepository, IContactTypeValidation _contacttypeValidation)
        {
            _repository = _contacttypeRepository;
            _validator = _contacttypeValidation;
        }

        public IQueryable<ContactType> GetQueryable()
        {  
            return _repository.GetQueryable();
        }

        public ContactType GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }
         
         
        public ContactType UpdateObject(ContactType contacttype)
        {
            if (!isValid(_validator.VUpdateObject(contacttype, this)))
            {
                contacttype = _repository.UpdateObject(contacttype);
            }
            return contacttype;
        }
         
        public ContactType SoftDeleteObject(ContactType contacttype)
        {
            contacttype = _repository.SoftDeleteObject(contacttype);
            return contacttype;
        }



        public bool isValid(ContactType obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }
    }
}
