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
    public class EmployeeService : IEmployeeService 
    {  
        private IEmployeeRepository _repository;
        private IEmployeeValidation _validator;

        public EmployeeService(IEmployeeRepository _employeeRepository, IEmployeeValidation _employeeValidation)
        {
            _repository = _employeeRepository;
            _validator = _employeeValidation;
        }

        public IQueryable<Employee> GetQueryable()
        {  
            return _repository.GetQueryable();
        }

        public Employee GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }
         
        public Employee CreateObject(Employee employee)
        {
            employee.Errors = new Dictionary<String, String>();
            if (isValid(_validator.VCreateObject(employee,this)))
            {
                employee.MasterCode = _repository.GetLastMasterCode(employee.OfficeId) + 1;
                employee = _repository.CreateObject(employee);
            }
            return employee;
        }
         
        public Employee UpdateObject(Employee employee)
        {
            if (isValid(_validator.VUpdateObject(employee, this)))
            {
                employee = _repository.UpdateObject(employee);
            }
            return employee;
        }
         
        public Employee SoftDeleteObject(Employee employee)
        {
            employee = _repository.SoftDeleteObject(employee);
            return employee;
        }

        public bool IsNameDuplicated(Employee employee)
        {
            return _repository.IsNameDuplicated(employee);
        }


        public bool isValid(Employee obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }
    }
}
