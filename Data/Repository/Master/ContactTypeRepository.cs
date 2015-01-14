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
    public class ContactTypeRepository : EfRepository<ContactType>, IContactTypeRepository
    {
        private ExpedicoEntities entities;

        public ContactTypeRepository()
        { 
            entities = new ExpedicoEntities();
        }

        public IQueryable<ContactType> GetQueryable()
        {
            return FindAll();
        }

        public ContactType GetObjectById(int Id)
        {
            ContactType data = Find(x => x.Id == Id && !x.IsDeleted);
            if (data != null) { data.Errors = new Dictionary<string, string>(); }
            return data;
        }

        public ContactType CreateObject(ContactType model)
        {
            model.IsDeleted = false;
            model.CreatedAt = DateTime.Now;
            return Create(model);
        }

        public ContactType UpdateObject(ContactType model)
        {
            model.UpdatedAt = DateTime.Now;
            Update(model);
            return model;
        }

        public ContactType SoftDeleteObject(ContactType model)
        {
            model.IsDeleted = true;
            model.DeletedAt = DateTime.Now;
            Update(model);
            return model;
        }

        public bool DeleteObject(int Id)
        {
            ContactType data = Find(x => x.Id == Id);
            return (Delete(data) == 1) ? true : false;
        }
    }
}
