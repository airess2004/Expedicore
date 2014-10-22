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
    public class CostRepository : EfRepository<Cost>, ICostRepository
    {
        private ExpedicoEntities entities;

        public CostRepository()
        { 
            entities = new ExpedicoEntities();
        }

        public IQueryable<Cost> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public Cost GetObjectById(int Id)
        {
            Cost data = Find(x => x.Id == Id && !x.IsDeleted);
            if (data != null) { data.Errors = new Dictionary<string, string>(); }
            return data;
        }

        public int GetLastMasterCode(int officeId)
        {
             int? data = FindAll(x => x.OfficeId == officeId).Max().MasterCode;
            if (data != null)
            {
                return data.Value;
            }
            else
            {
                return 1;
            }
        }

        public Cost CreateObject(Cost model)
        {
            model.IsDeleted = false;
            model.CreatedAt = DateTime.Now;
            return Create(model);
        }

        public Cost UpdateObject(Cost model)
        {
            model.UpdatedAt = DateTime.Now;
            Update(model);
            return model;
        }

        public Cost SoftDeleteObject(Cost model)
        {
            model.IsDeleted = true;
            model.DeletedAt = DateTime.Now;
            Update(model);
            return model;
        }

        public bool DeleteObject(int Id)
        {
            Cost data = Find(x => x.Id == Id);
            return (Delete(data) == 1) ? true : false;
        }

        public bool IsNameDuplicated(Cost model)
        {
            IQueryable<Cost> items = FindAll(x => x.Name == model.Name && !x.IsDeleted && x.Id != model.Id && x.OfficeId == model.OfficeId);
            return (items.Count() > 0 ? true : false);
        }
    }
}
