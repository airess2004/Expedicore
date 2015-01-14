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
    public class OfficeRepository : EfRepository<Office>, IOfficeRepository
    {
        private ExpedicoEntities entities;

        public OfficeRepository()
        { 
            entities = new ExpedicoEntities();
        }

        public IQueryable<Office> GetQueryable()
        {
            return FindAll();
        }

        public Office GetObjectById(int Id)
        {
            Office data = Find(x => x.Id == Id && !x.IsDeleted);
            if (data != null) { data.Errors = new Dictionary<string, string>(); }
            return data;
        }

        public Office CreateObject(Office model)
        {
            model.IsDeleted = false;
            model.CreatedAt = DateTime.Now;
            return Create(model);
        }

        public Office UpdateObject(Office model)
        {
            model.UpdatedAt = DateTime.Now;
            Update(model);
            return model;
        }

        public Office SoftDeleteObject(Office model)
        {
            model.IsDeleted = true;
            model.DeletedAt = DateTime.Now;
            Update(model);
            return model;
        }

        public bool DeleteObject(int Id)
        {
            Office data = Find(x => x.Id == Id);
            return (Delete(data) == 1) ? true : false;
        }

        public bool IsNameDuplicated(Office model)
        { 
            IQueryable<Office> items = FindAll(x => x.Name == model.Name && !x.IsDeleted && x.Id != model.Id);
            return (items.Count() > 0 ? true : false);
        }
    }
}
