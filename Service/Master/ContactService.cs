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
    public class ContactService : IContactService 
    {  
        private IContactRepository _repository;
        private IContactValidation _validator;

        public ContactService(IContactRepository _contactRepository, IContactValidation _contactValidation)
        {
            _repository = _contactRepository;
            _validator = _contactValidation;
        }

        public IQueryable<Contact> GetQueryable()
        {  
            return _repository.GetQueryable();
        }

        public Contact GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }
         
        public Contact CreateObject(Contact contact)
        {
            contact.Errors = new Dictionary<String, String>();
            if (isValid(_validator.VCreateObject(contact,this)))
            {
                contact.MasterCode = _repository.GetLastMasterCode(contact.OfficeId) + 1;
                contact = _repository.CreateObject(contact);
            }
            return contact;
        }

        public Contact UpdateObject(Contact contact)
        {
            if (isValid(_validator.VUpdateObject(contact, this)))
            {
                contact = _repository.UpdateObject(contact);
            }
            return contact;
        }

        public Contact UpdateLastShipment(Contact contact)
        {
            contact = _repository.UpdateObject(contact);
            return contact;
        }

        public Contact SoftDeleteObject(Contact contact)
        {
            contact = _repository.SoftDeleteObject(contact);
            return contact;
        }

        public bool IsNameDuplicated(Contact contact)
        {
            return _repository.IsNameDuplicated(contact);
        }


        public bool isValid(Contact obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

    }
}
