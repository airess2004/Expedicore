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
    public class GroupContactRepository : EfRepository<GroupContact>, IGroupContactRepository
    {
        private ExpedicoEntities entities;

        public GroupContactRepository()
        { 
            entities = new ExpedicoEntities();
        }

        public IQueryable<GroupContact> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public GroupContact GetObjectById(int Id)
        {
            GroupContact data = Find(x => x.Id == Id && !x.IsDeleted);
            if (data != null) { data.Errors = new Dictionary<string, string>(); }
            return data;
        }

        public GroupContact CreateObject(GroupContact model)
        {
            model.IsDeleted = false;
            model.CreatedAt = DateTime.Now;
            return Create(model);
        }

        public GroupContact UpdateObject(GroupContact model)
        {
            model.UpdatedAt = DateTime.Now;
            Update(model);
            return model;
        }

        public GroupContact SoftDeleteObject(GroupContact model)
        {
            model.IsDeleted = true;
            model.DeletedAt = DateTime.Now;
            Update(model);
            return model;
        }

        public bool DeleteObject(int Id)
        {
            GroupContact data = Find(x => x.Id == Id);
            return (Delete(data) == 1) ? true : false;
        }
    }
}
