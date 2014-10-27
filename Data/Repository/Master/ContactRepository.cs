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

        public int GetLastMasterCode(int officeId)
        {
            Contact data = FindAll(x => x.OfficeId == officeId).OrderByDescending(x => x.MasterCode).FirstOrDefault();
            if (data != null)
            {
                return data.MasterCode;
            }
            else
            {
                return 0;
            }
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

        public bool IsNameDuplicated(Contact model)
        {
            IQueryable<Contact> items = FindAll(x => x.ContactName == model.ContactName && !x.IsDeleted && x.Id != model.Id && x.OfficeId == model.OfficeId);
            return (items.Count() > 0 ? true : false);
        }
    }
}
