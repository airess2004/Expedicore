using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Validation
{
    public interface IEmployeeValidation
    {
        Employee VCreateObject(Employee employee, IEmployeeService _employeeService);
        Employee VUpdateObject(Employee employee, IEmployeeService _employeeService);
    }
}
