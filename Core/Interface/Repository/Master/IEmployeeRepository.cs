using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
       IQueryable<Employee> GetQueryable();
       Employee GetObjectById(int Id);
       Employee CreateObject(Employee model);
       Employee UpdateObject(Employee model);
       Employee SoftDeleteObject(Employee model);
       bool DeleteObject(int Id);
       bool IsNameDuplicated(Employee model);
       int GetLastMasterCode(int officeId);
    }
}