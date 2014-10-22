using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validation.Validation
{ 
    public class ContactTypeValidation : IContactTypeValidation
    {  
        public ContactType VName(ContactType contacttype, IContactTypeService _contacttypeService)
        {
            if (String.IsNullOrEmpty(contacttype.Name) || contacttype.Name.Trim() == "")
            {
                contacttype.Errors.Add("Name", "Tidak boleh kosong");
            }
            return contacttype;
        }

        
        public ContactType VCreateObject(ContactType contacttype, IContactTypeService _contacttypeService)
        {
            VName(contacttype, _contacttypeService);
            if (!isValid(contacttype)) { return contacttype; }
            return contacttype;
        }

        public ContactType VUpdateObject(ContactType contacttype, IContactTypeService _contacttypeService)
        { 
            VName(contacttype, _contacttypeService);
            if (!isValid(contacttype)) { return contacttype; }
            return contacttype;
        }

        public bool isValid(ContactType obj)
        { 
            bool isValid = !obj.Errors.Any();
            return isValid;
        }
    }
}
