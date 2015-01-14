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
    public class ContinentRepository : EfRepository<Continent>, IContinentRepository
    {
        private ExpedicoEntities entities;

        public ContinentRepository()
        { 
            entities = new ExpedicoEntities();
        }

        public IQueryable<Continent> GetQueryable()
        {
            return FindAll();
        }

        public Continent GetObjectById(int Id)
        {
            Continent data = Find(x => x.Id == Id && !x.IsDeleted);
            if (data != null) { data.Errors = new Dictionary<string, string>(); }
            return data;
        }
         
        public int GetLastMasterCode(int officeId) 
        { 
            Continent data = FindAll(x => x.OfficeId == officeId).OrderByDescending(x => x.MasterCode).FirstOrDefault();
            if (data != null)
            {
                return data.MasterCode;
            }
            else
            {
                return 0;
            }
        }

        public Continent CreateObject(Continent model)
        {
            model.IsDeleted = false;
            model.CreatedAt = DateTime.Now;
            return Create(model);
        }

        public Continent UpdateObject(Continent model)
        {
            model.UpdatedAt = DateTime.Now;
            Update(model);
            return model;
        }

        public Continent SoftDeleteObject(Continent model)
        {
            model.IsDeleted = true;
            model.DeletedAt = DateTime.Now;
            Update(model);
            return model;
        }

        public bool DeleteObject(int Id)
        {
            Continent data = Find(x => x.Id == Id);
            return (Delete(data) == 1) ? true : false;
        }

        public bool IsNameDuplicated(Continent model)
        {
            IQueryable<Continent> items = FindAll(x => x.Name == model.Name && !x.IsDeleted && x.Id != model.Id && x.OfficeId == model.OfficeId);
            return (items.Count() > 0 ? true : false);
        }


    }
}
