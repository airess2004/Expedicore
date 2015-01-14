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
    public class AirportRepository : EfRepository<Airport>, IAirportRepository
    {
        private ExpedicoEntities entities;

        public AirportRepository()
        { 
            entities = new ExpedicoEntities();
        }

        public IQueryable<Airport> GetQueryable()
        {
            return FindAll();
        }

        public Airport GetObjectById(int Id)
        {
            Airport data = Find(x => x.Id == Id && !x.IsDeleted);
            if (data != null) { data.Errors = new Dictionary<string, string>(); }
            return data;
        }

        public int GetLastMasterCode(int officeId)
        {
            Airport data = FindAll(x => x.OfficeId == officeId).OrderByDescending(x => x.MasterCode).FirstOrDefault();
            if (data != null)
            {
                return data.MasterCode;
            }
            else
            {
                return 0;
            }
        }

        public Airport CreateObject(Airport model)
        {
            model.IsDeleted = false;
            model.CreatedAt = DateTime.Now;
            return Create(model);
        }

        public Airport UpdateObject(Airport model)
        {
            model.UpdatedAt = DateTime.Now;
            Update(model);
            return model;
        }

        public Airport SoftDeleteObject(Airport model)
        {
            model.IsDeleted = true;
            model.DeletedAt = DateTime.Now;
            Update(model);
            return model;
        }

        public bool DeleteObject(int Id)
        {
            Airport data = Find(x => x.Id == Id);
            return (Delete(data) == 1) ? true : false;
        }

        public bool IsNameDuplicated(Airport model)
        {
            IQueryable<Airport> items = FindAll(x => x.Name == model.Name && !x.IsDeleted && x.Id != model.Id && x.OfficeId == model.OfficeId);
            return (items.Count() > 0 ? true : false);
        }
    }
}
