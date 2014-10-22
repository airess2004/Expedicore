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
            if (String.IsNullOrEmpty(contact.Name) || contact.Name.Trim() == "")
            {
                contact.Errors.Add("Name", "Tidak boleh kosong");
            }
            else if (_contactService.IsNameDuplicated(contact))
            {
                contact.Errors.Add("Name", "Tidak boleh diduplikasi");
            }
            return contact;
        }
         
        public Contact VStatus(Contact contact)
        {
            if (String.IsNullOrEmpty(contact.ContactStatus) || contact.ContactStatus.Trim() == "")
            {
                contact.Errors.Add("Status", "Tidak boleh kosong");
            }
            if (contact.Name != MasterConstant.ContactStatus.PT || 
                contact.Name != MasterConstant.ContactStatus.CV || 
                contact.Name != MasterConstant.ContactStatus.Other)
            {
                contact.Errors.Add("Status", "Status Input Error");
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

        public Contact VCity(Contact contact,ICityLocationService _citylocationService)
        {
            CityLocation citylocation = _citylocationService.GetObjectById(contact.CityId);
            if (citylocation == null)
            {
                contact.Errors.Add("City", "Tidak boleh kosong");
            }
            else if (!VOffice(citylocation.OfficeId, contact.OfficeId))
            {
                citylocation.Errors.Add("Country", "Invalid Country");
            }
            return contact;
        }


        public Contact VType(Contact contact)
        {
            if (contact.IsAgent == false &&
                contact.IsConsignee == false &&
                contact.IsDepo == false &&
                contact.IsEMKL == false &&
                contact.IsIATA == false &&
                contact.IsSSLine == false)
            {
                contact.Errors.Add("ContactType", "Tidak boleh kosong");
            }
            return contact;
        }

        public Contact VCreateObject(Contact contact, IContactService _contactService, ICityLocationService _citylocationService)
        {
            VName(contact, _contactService);
            if (!isValid(contact)) { return contact; }
            VStatus(contact);
            if (!isValid(contact)) { return contact; }
            VCity(contact,_citylocationService);
            if (!isValid(contact)) { return contact; }
            VAddress(contact);
            if (!isValid(contact)) { return contact; }
            VType(contact);
            if (!isValid(contact)) { return contact; }
            return contact;
        }
         
        public Contact VUpdateObject(Contact contact, IContactService _contactService, ICityLocationService _citylocationService)
        {
            VObject(contact, _contactService);
            if (!isValid(contact)) { return contact; }
            VName(contact, _contactService);
            if (!isValid(contact)) { return contact; }
            VStatus(contact);
            if (!isValid(contact)) { return contact; }
            VCity(contact, _citylocationService);
            if (!isValid(contact)) { return contact; }
            VAddress(contact);
            if (!isValid(contact)) { return contact; }
            VType(contact);
            if (!isValid(contact)) { return contact; }
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
