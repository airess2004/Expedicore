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
    public class EmployeeValidation : IEmployeeValidation
    {  
        public Employee VName(Employee employee, IEmployeeService _employeeService)
        {
            if (String.IsNullOrEmpty(employee.Name) || employee.Name.Trim() == "")
            {
                employee.Errors.Add("Name", "Tidak boleh kosong");
            }
            else if (_employeeService.IsNameDuplicated(employee))
            {
                employee.Errors.Add("Name", "Tidak boleh diduplikasi");
            }
            return employee;
        }

        
        public Employee VCreateObject(Employee employee, IEmployeeService _employeeService)
        {
            VName(employee, _employeeService);
            if (!isValid(employee)) { return employee; }
            return employee;
        }

        public Employee VUpdateObject(Employee employee, IEmployeeService _employeeService)
        {
            VObject(employee, _employeeService);
            if (!isValid(employee)) { return employee; }
            VName(employee, _employeeService);
            if (!isValid(employee)) { return employee; }
            return employee;
        }

        public bool isValid(Employee obj)
        { 
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public Employee VObject(Employee employee, IEmployeeService _employeeService)
        {
            Employee oldemployee = _employeeService.GetObjectById(employee.Id);
            if (oldemployee == null)
            {
                employee.Errors.Add("Generic", "Invalid Data For Update");
            }
            else if (!VOffice(employee.OfficeId, oldemployee.OfficeId))
            {
                employee.Errors.Add("Generic", "Invalid Data For Update");
            }
            return employee;
        }

        public bool VOffice(int OfficeId, int CekOfficeId)
        {
            return OfficeId == CekOfficeId;
        }

    }
}
