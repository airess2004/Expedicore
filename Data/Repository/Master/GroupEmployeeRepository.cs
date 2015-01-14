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
    public class GroupEmployeeRepository : EfRepository<GroupEmployee>, IGroupEmployeeRepository
    {
        private ExpedicoEntities entities;

        public GroupEmployeeRepository()
        { 
            entities = new ExpedicoEntities();
        }

        public IQueryable<GroupEmployee> GetQueryable()
        {
            return FindAll();
        }

        public GroupEmployee GetObjectById(int Id)
        {
            GroupEmployee data = Find(x => x.Id == Id && !x.IsDeleted);
            if (data != null) { data.Errors = new Dictionary<string, string>(); }
            return data;
        }
         
        public int GetLastMasterCode(int officeId) 
        {
            GroupEmployee data = FindAll(x => x.OfficeId == officeId).OrderByDescending(x => x.MasterCode).FirstOrDefault();
            if (data != null)
            {
                return data.MasterCode;
            }
            else
            {
                return 0;
            }
        }

        public GroupEmployee CreateObject(GroupEmployee model)
        {
            model.IsDeleted = false;
            model.CreatedAt = DateTime.Now;
            return Create(model);
        }

        public GroupEmployee UpdateObject(GroupEmployee model)
        {
            model.UpdatedAt = DateTime.Now;
            Update(model);
            return model;
        }

        public GroupEmployee SoftDeleteObject(GroupEmployee model)
        {
            model.IsDeleted = true;
            model.DeletedAt = DateTime.Now;
            Update(model);
            return model;
        }

        public bool DeleteObject(int Id)
        {
            GroupEmployee data = Find(x => x.Id == Id);
            return (Delete(data) == 1) ? true : false;
        }

        public bool IsNameDuplicated(GroupEmployee model)
        {
            IQueryable<GroupEmployee> items = FindAll(x => x.Name == model.Name && !x.IsDeleted && x.Id != model.Id && x.OfficeId == model.OfficeId);
            return (items.Count() > 0 ? true : false);
        }


    }
}
