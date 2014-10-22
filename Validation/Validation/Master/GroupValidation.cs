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
    public class GroupValidation : IGroupValidation
    {  
        public Group VName(Group group, IGroupService _groupService)
        {
            if (String.IsNullOrEmpty(group.Name) || group.Name.Trim() == "")
            {
                group.Errors.Add("Name", "Tidak boleh kosong");
            }
            if (_groupService.IsNameDuplicated(group))
            {
                group.Errors.Add("Name", "Tidak boleh diduplikasi");
            }
            return group;
        }

        
        public Group VCreateObject(Group group, IGroupService _groupService)
        {
            VName(group, _groupService);
            if (!isValid(group)) { return group; }
            return group;
        }

        public Group VUpdateObject(Group group, IGroupService _groupService)
        { 
            VName(group, _groupService);
            if (!isValid(group)) { return group; }
            return group;
        }

        public bool isValid(Group obj)
        { 
            bool isValid = !obj.Errors.Any();
            return isValid;
        }
    }
}
