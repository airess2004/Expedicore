using Core.DomainModel;
using Core.Constant;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validation.Validation
{ 
    public class ContactValidation : IContactValidation
    {  
        public Contact VName(Contact contact, IContactService _contactService)
        {
            if (String.IsNullOrEmpty(contact.ContactName) || contact.ContactName.Trim() == "")
            {
                contact.Errors.Add("ContactName", "Tidak boleh kosong");
            }
            else if (_contactService.IsNameDuplicated(contact))
            {
                contact.Errors.Add("ContactName", "Tidak boleh diduplikasi");
            }
            return contact;
        }
         
       

        public Contact VAddress(Contact contact)
        {
            if (String.IsNullOrEmpty(contact.ContactAddress) || contact.ContactAddress.Trim() == "")
            {
                contact.Errors.Add("Address", "Tidak boleh kosong");
            }
            return contact;
        }

        public Contact VCreateObject(Contact contact, IContactService _contactService)
        {
            VName(contact, _contactService);
            if (!isValid(contact)) { return contact; }
            VAddress(contact);
            if (!isValid(contact)) { return contact; }
            return contact;
        }
         
        public Contact VUpdateObject(Contact contact, IContactService _contactService)
        {
            VObject(contact, _contactService);
            if (!isValid(contact)) { return contact; }
            VName(contact, _contactService);
            if (!isValid(contact)) { return contact; }
            VAddress(contact);
            if (isValid(contact)) { return contact; }
            return contact;
        }

        public bool isValid(Contact obj)
        { 
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public Contact VObject(Contact contact, IContactService _contactService)
        {
            Contact oldcontact = _contactService.GetObjectById(contact.Id);
            if (oldcontact == null)
            {
                contact.Errors.Add("Generic", "Invalid Data For Update");
            }
            else if (!VOffice(contact.OfficeId, oldcontact.OfficeId))
            {
                contact.Errors.Add("Generic", "Invalid Data For Update");
            }
            return contact;
        }

        public bool VOffice(int OfficeId, int CekOfficeId)
        {
            return OfficeId == CekOfficeId;
        }

    }
}
