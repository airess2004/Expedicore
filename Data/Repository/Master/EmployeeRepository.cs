using Core.DomainModel;
using Core.Interface.Repository;
using Data.Context;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository
{ 
    public class EmployeeRepository : EfRepository<Employee>, IEmployeeRepository
    {
        private ExpedicoEntities entities;

        public EmployeeRepository()
        { 
            entities = new ExpedicoEntities();
        }

        public IQueryable<Employee> GetQueryable()
        {
            return FindAll();
        }

        public Employee GetObjectById(int Id)
        {
            Employee data = Find(x => x.Id == Id && !x.IsDeleted);
            if (data != null) { data.Errors = new Dictionary<string, string>(); }
            return data;
        }
         
        public int GetLastMasterCode(int officeId) 
        {
            Employee data = FindAll(x => x.OfficeId == officeId).OrderByDescending(x => x.MasterCode).FirstOrDefault();
            if (data != null)
            {
                return data.MasterCode;
            }
            else
            {
                return 0;
            }
        }

        public Employee CreateObject(Employee model)
        {
            model.IsDeleted = false;
            model.CreatedAt = DateTime.Now;
            return Create(model);
        }

        public Employee UpdateObject(Employee model)
        {
            model.UpdatedAt = DateTime.Now;
            Update(model);
            return model;
        }

        public Employee SoftDeleteObject(Employee model)
        {
            model.IsDeleted = true;
            model.DeletedAt = DateTime.Now;
            Update(model);
            return model;
        }

        public bool DeleteObject(int Id)
        {
            Employee data = Find(x => x.Id == Id);
            return (Delete(data) == 1) ? true : false;
        }

        public bool IsNameDuplicated(Employee model)
        {
            IQueryable<Employee> items = FindAll(x => x.Name == model.Name && !x.IsDeleted && x.Id != model.Id && x.OfficeId == model.OfficeId);
            return (items.Count() > 0 ? true : false);
        }


    }
}
