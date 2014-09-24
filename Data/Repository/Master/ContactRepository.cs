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
    public class ContactRepository : EfRepository<Contact>, IContactRepository
    {
        private ExpedicoEntities entities;

        public ContactRepository()
        { 
            entities = new ExpedicoEntities();
        }

        public IQueryable<Contact> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public Contact GetObjectById(int Id)
        {
            Contact data = Find(x => x.Id == Id && !x.IsDeleted);
            if (data != null) { data.Errors = new Dictionary<string, string>(); }
            return data;
        }

        public Contact CreateObject(Contact model)
        {
            model.IsDeleted = false;
            model.CreatedAt = DateTime.Now;
            return Create(model);
        }

        public Contact UpdateObject(Contact model)
        {
            model.UpdatedAt = DateTime.Now;
            Update(model);
            return model;
        }

        public Contact SoftDeleteObject(Contact model)
        {
            model.IsDeleted = true;
            model.DeletedAt = DateTime.Now;
            Update(model);
            return model;
        }

        public bool DeleteObject(int Id)
        {
            Contact data = Find(x => x.Id == Id);
            return (Delete(data) == 1) ? true : false;
        }
    }
}
