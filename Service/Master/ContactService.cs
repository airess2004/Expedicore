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
         
        public Contact CreateObject(Contact contact,ICityLocationService _citylocationService)
        {
            contact.Errors = new Dictionary<String, String>();
            if (!isValid(_validator.VCreateObject(contact,this,_citylocationService)))
            {
                contact.MasterCode = _repository.GetLastMasterCode(contact.OfficeId) + 1;
                contact.AgentCode = (contact.IsAgent) ? (_repository.GetLastAgentCode(contact.OfficeId) + 1) : contact.AgentCode;
                if (contact.IsConsignee == true)
                {
                    contact.ConsigneeCode = _repository.GetLastConsigneeCode(contact.OfficeId) + 1;
                }
                if (contact.IsDepo == true)
                {
                    contact.DepoCode = _repository.GetLastDepoCode(contact.OfficeId) + 1;
                }
                if (contact.IsEMKL == true)
                {
                    contact.EMKLCode = _repository.GetLastEMKLCode(contact.OfficeId) + 1;
                }
                if (contact.IsIATA == true)
                {
                    contact.IATACode = _repository.GetLastIATACode(contact.OfficeId) + 1;
                }
                if (contact.IsShipper == true)
                {
                    contact.ShipperCode = _repository.GetLastShipperCode(contact.OfficeId) + 1;
                }
                if (contact.IsSSLine == true)
                {
                    contact.SSLineCode = _repository.GetLastSSLineCode(contact.OfficeId) + 1;
                }
                contact = _repository.CreateObject(contact);
            }
            return contact;
        }

        public Contact UpdateObject(Contact contact, ICityLocationService _citylocationService)
        {
            if (!isValid(_validator.VUpdateObject(contact, this,_citylocationService)))
            {
                if (contact.IsAgent == true && contact.AgentCode == null)
                {
                    contact.AgentCode = _repository.GetLastAgentCode(contact.OfficeId) + 1;
                }
                if (contact.IsConsignee == true && contact.ConsigneeCode == null)
                {
                    contact.ConsigneeCode = _repository.GetLastConsigneeCode(contact.OfficeId) + 1;
                }
                if (contact.IsDepo == true && contact.DepoCode == null)
                {
                    contact.DepoCode = _repository.GetLastDepoCode(contact.OfficeId) + 1;
                }
                if (contact.IsEMKL == true && contact.EMKLCode == null)
                {
                    contact.EMKLCode = _repository.GetLastEMKLCode(contact.OfficeId) + 1;
                }
                if (contact.IsIATA == true && contact.IATACode == null)
                {
                    contact.IATACode = _repository.GetLastIATACode(contact.OfficeId) + 1;
                }
                if (contact.IsShipper == true && contact.ShipperCode == null)
                {
                    contact.ShipperCode = _repository.GetLastShipperCode(contact.OfficeId) + 1;
                }
                if (contact.IsSSLine == true && contact.SSLineCode == null)
                {
                    contact.SSLineCode = _repository.GetLastSSLineCode(contact.OfficeId) + 1;
                }
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
