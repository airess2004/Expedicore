using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IEmployeeService
    {
        IQueryable<Employee> GetQueryable();
        Employee GetObjectById(int Id);
        Employee CreateObject(Employee continent);
        Employee UpdateObject(Employee continent);
        Employee SoftDeleteObject(Employee continent);
        bool IsNameDuplicated(Employee continent);
    }
}