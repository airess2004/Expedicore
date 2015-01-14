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
    public class TelexReleaseRepository : EfRepository<TelexRelease>, ITelexReleaseRepository 
    {
        private ExpedicoEntities entities;
         
        public TelexReleaseRepository()
        { 
            entities = new ExpedicoEntities();
        }

        public IQueryable<TelexRelease> GetQueryable()
        {
            return FindAll();
        }

        public TelexRelease GetObjectById(int Id)
        {
            TelexRelease data = Find(x => x.Id == Id && !x.IsDeleted);
            if (data != null) { data.Errors = new Dictionary<string, string>(); }
            return data;
        }

        public TelexRelease GetObjectByShipmentOrderId(int Id)
        {
            TelexRelease data = Find(x => x.ShipmentOrderId == Id && !x.IsDeleted);
            if (data != null) { data.Errors = new Dictionary<string, string>(); }
            return data;
        }


        public TelexRelease CreateObject(TelexRelease model)
        {
            model.IsDeleted = false;
            model.CreatedAt = DateTime.Now;
            return Create(model);
        }

        public TelexRelease UpdateObject(TelexRelease model)
        {
            model.UpdatedAt = DateTime.Now;
            Update(model);
            return model;
        }

        public TelexRelease SoftDeleteObject(TelexRelease model)
        {
            model.IsDeleted = true;
            model.DeletedAt = DateTime.Now;
            Update(model);
            return model;
        }

        public bool DeleteObject(int Id)
        {
            TelexRelease data = Find(x => x.Id == Id);
            return (Delete(data) == 1) ? true : false;
        }

    }
}
