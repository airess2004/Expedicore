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
    public class GroupEmployeeValidation : IGroupEmployeeValidation
    {  
        public GroupEmployee VName(GroupEmployee groupEmployee, IGroupEmployeeService _groupEmployeeService)
        {
            if (String.IsNullOrEmpty(groupEmployee.Name) || groupEmployee.Name.Trim() == "")
            {
                groupEmployee.Errors.Add("Name", "Tidak boleh kosong");
            }
            else if (_groupEmployeeService.IsNameDuplicated(groupEmployee))
            {
                groupEmployee.Errors.Add("Name", "Tidak boleh diduplikasi");
            }
            return groupEmployee;
        }

        
        public GroupEmployee VCreateObject(GroupEmployee groupEmployee, IGroupEmployeeService _groupEmployeeService)
        {
            VName(groupEmployee, _groupEmployeeService);
            if (!isValid(groupEmployee)) { return groupEmployee; }
            return groupEmployee;
        }

        public GroupEmployee VUpdateObject(GroupEmployee groupEmployee, IGroupEmployeeService _groupEmployeeService)
        {
            VObject(groupEmployee, _groupEmployeeService);
            if (!isValid(groupEmployee)) { return groupEmployee; }
            VName(groupEmployee, _groupEmployeeService);
            if (!isValid(groupEmployee)) { return groupEmployee; }
            return groupEmployee;
        }

        public bool isValid(GroupEmployee obj)
        { 
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public GroupEmployee VObject(GroupEmployee groupEmployee, IGroupEmployeeService _groupEmployeeService)
        {
            GroupEmployee oldgroupEmployee = _groupEmployeeService.GetObjectById(groupEmployee.Id);
            if (oldgroupEmployee == null)
            {
                groupEmployee.Errors.Add("Generic", "Invalid Data For Update");
            }
            else if (!VOffice(groupEmployee.OfficeId, oldgroupEmployee.OfficeId))
            {
                groupEmployee.Errors.Add("Generic", "Invalid Data For Update");
            }
            return groupEmployee;
        }

        public bool VOffice(int OfficeId, int CekOfficeId)
        {
            return OfficeId == CekOfficeId;
        }

    }
}
