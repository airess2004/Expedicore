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
    public class BillOfLadingRepository : EfRepository<BillOfLading>, IBillOfLadingRepository 
    {
        private ExpedicoEntities entities;
         
        public BillOfLadingRepository()
        { 
            entities = new ExpedicoEntities();
        }

        public IQueryable<BillOfLading> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }


        public BillOfLading GetObjectById(int Id)
        {
            BillOfLading data = Find(x => x.Id == Id && !x.IsDeleted);
            if (data != null) { data.Errors = new Dictionary<string, string>(); }
            return data;
        }
          
        public BillOfLading GetObjectByShipmentOrderId(int Id)
        {
            BillOfLading data = Find(x => x.ShipmentOrderId == Id && !x.IsDeleted);
            if (data != null) { data.Errors = new Dictionary<string, string>(); }
            return data;
        }

        public BillOfLading CreateObject(BillOfLading model)
        {
            model.IsDeleted = false;
            model.CreatedAt = DateTime.Now;
            model.UpdatedAt = null;
            model.UpdatedBy = null;
            return Create(model);
        }

        public BillOfLading UpdateObject(BillOfLading model)
        {
            model.UpdatedAt = DateTime.Now;
            Update(model);
            return model;
        }

        public BillOfLading SoftDeleteObject(BillOfLading model)
        {
            model.IsDeleted = true;
            model.DeletedAt = DateTime.Now;
            Update(model);
            return model;
        }

        public bool DeleteObject(int Id)
        {
            BillOfLading data = Find(x => x.Id == Id);
            return (Delete(data) == 1) ? true : false;
        }

    }
}
