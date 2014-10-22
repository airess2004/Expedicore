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
    public class GroupContactValidation : IGroupContactValidation
    {  
        public GroupContact VName(GroupContact groupcontact, IGroupContactService _groupcontactService)
        {
            if (String.IsNullOrEmpty(groupcontact.Name) || groupcontact.Name.Trim() == "")
            {
                groupcontact.Errors.Add("Name", "Tidak boleh kosong");
            }
            if (_groupcontactService.IsNameDuplicated(groupcontact))
            {
                groupcontact.Errors.Add("Name", "Tidak boleh diduplikasi");
            }
            return groupcontact;
        }

        public GroupContact VAbbrevation(GroupContact groupcontact, IGroupContactService _groupcontactService)
        {
            if (String.IsNullOrEmpty(groupcontact.Abbrevation) || groupcontact.Abbrevation.Trim() == "")
            {
                groupcontact.Errors.Add("Abbrevation", "Tidak boleh kosong");
            }
            return groupcontact;
        }
        
        public GroupContact VCreateObject(GroupContact groupcontact, IGroupContactService _groupcontactService)
        {
            VName(groupcontact, _groupcontactService);
            if (!isValid(groupcontact)) { return groupcontact; }
            VAbbrevation(groupcontact, _groupcontactService);
            if (!isValid(groupcontact)) { return groupcontact; }
            return groupcontact;
        }

        public GroupContact VUpdateObject(GroupContact groupcontact, IGroupContactService _groupcontactService)
        { 
            VName(groupcontact, _groupcontactService);
            if (!isValid(groupcontact)) { return groupcontact; }
            VAbbrevation(groupcontact, _groupcontactService);
            if (!isValid(groupcontact)) { return groupcontact; }
            return groupcontact;
        }

        public bool isValid(GroupContact obj)
        { 
            bool isValid = !obj.Errors.Any();
            return isValid;
        }
    }
}
