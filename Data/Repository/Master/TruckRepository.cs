using Core.DomainModel;
using Core.Interface.Repository;
using Data.Context;
using System;
using System.Data;
using System.Linq.Dynamic;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository
{ 
    public class TruckRepository : EfRepository<Truck>, ITruckRepository
    {
        private ExpedicoEntities entities;

        public TruckRepository()
        { 
            entities = new ExpedicoEntities();
        }

        public IQueryable<Truck> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public Truck GetObjectById(int Id)
        {
            Truck data = Find(x => x.Id == Id && !x.IsDeleted);
            if (data != null) { data.Errors = new Dictionary<string, string>(); }
            return data;
        }

        public int GetLastMasterCode(int officeId)
        {
           Truck data = FindAll(x => x.OfficeId == officeId).OrderByDescending(x=>x.MasterCode).FirstOrDefault();
            if (data != null)
            {
                return data.MasterCode;
            }
            else
            {
                return 0;
            }
        }


        public Truck CreateObject(Truck model)
        {
            model.IsDeleted = false;
            model.CreatedAt = DateTime.Now;
            return Create(model);
        }

        public Truck UpdateObject(Truck model)
        {
            model.UpdatedAt = DateTime.Now;
            Update(model);
            return model;
        }

        public Truck SoftDeleteObject(Truck model)
        {
            model.IsDeleted = true;
            model.DeletedAt = DateTime.Now;
            Update(model);
            return model;
        }

        public bool DeleteObject(int Id)
        {
            Truck data = Find(x => x.Id == Id);
            return (Delete(data) == 1) ? true : false;
        }

        public bool IsNameDuplicated(Truck model)
        {
            IQueryable<Truck> items = FindAll(x => x.NoPlat == model.NoPlat && !x.IsDeleted && x.Id != model.Id && x.OfficeId == model.OfficeId);
            return (items.Count() > 0 ? true : false);
        }
    }
}
