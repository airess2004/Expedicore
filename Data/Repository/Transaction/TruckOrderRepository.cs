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
    public class TruckOrderRepository : EfRepository<TruckOrder>, ITruckOrderRepository 
    {
        private ExpedicoEntities entities;
         
        public TruckOrderRepository()
        { 
            entities = new ExpedicoEntities();
        }

        public IQueryable<TruckOrder> GetQueryable()
        {
            return FindAll();
        }


        public TruckOrder GetObjectById(int Id)
        {
            TruckOrder data = Find(x => x.Id == Id && !x.IsDeleted);
            if (data != null) { data.Errors = new Dictionary<string, string>(); }
            return data;
        }


        public int GetLastMasterCode(int officeId)
        {
            TruckOrder data = FindAll(x => x.OfficeId == officeId).OrderByDescending(x => x.MasterCode).FirstOrDefault();
            if (data != null)
            {
                return data.MasterCode;
            }
            else
            {
                return 0;
            }
        }

        public TruckOrder CreateObject(TruckOrder model)
        {
            model.IsDeleted = false;
            model.CreatedAt = DateTime.Now;
            model.UpdatedAt = null;
            model.UpdatedBy = null;
            return Create(model);
        }

        public TruckOrder UpdateObject(TruckOrder model)
        {
            model.UpdatedAt = DateTime.Now;
            Update(model);
            return model;
        }

        public TruckOrder SoftDeleteObject(TruckOrder model)
        {
            model.IsDeleted = true;
            model.DeletedAt = DateTime.Now;
            Update(model);
            return model;
        }

        public bool DeleteObject(int Id)
        {
            TruckOrder data = Find(x => x.Id == Id);
            return (Delete(data) == 1) ? true : false;
        }

    }
}
