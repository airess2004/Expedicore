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
    public class CityLocationRepository : EfRepository<CityLocation>, ICityLocationRepository
    {
        private ExpedicoEntities entities;

        public CityLocationRepository()
        { 
            entities = new ExpedicoEntities();
        }

        public IQueryable<CityLocation> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public CityLocation GetObjectById(int Id)
        {
            CityLocation data = Find(x => x.Id == Id && !x.IsDeleted);
            if (data != null) { data.Errors = new Dictionary<string, string>(); }
            return data;
        }

        public int GetLastMasterCode(int officeId)
        {
            CityLocation data = FindAll(x => x.OfficeId == officeId).OrderByDescending(x => x.MasterCode).FirstOrDefault();
            if (data != null)
            {
                return data.MasterCode;
            }
            else
            {
                return 0;
            }
        }

       

        public CityLocation CreateObject(CityLocation model)
        {
            model.IsDeleted = false;
            model.CreatedAt = DateTime.Now;
            return Create(model);
        }

        public CityLocation UpdateObject(CityLocation model)
        {
            model.UpdatedAt = DateTime.Now;
            Update(model);
            return model;
        }

        public CityLocation SoftDeleteObject(CityLocation model)
        {
            model.IsDeleted = true;
            model.DeletedAt = DateTime.Now;
            Update(model);
            return model;
        }

        public bool DeleteObject(int Id)
        {
            CityLocation data = Find(x => x.Id == Id);
            return (Delete(data) == 1) ? true : false; 
        }

        public bool IsNameDuplicated(CityLocation model)
        {
            IQueryable<CityLocation> items = FindAll(x => x.Name == model.Name && !x.IsDeleted && x.Id != model.Id && x.OfficeId == model.OfficeId);
            return (items.Count() > 0 ? true : false);
        }
    }
}
