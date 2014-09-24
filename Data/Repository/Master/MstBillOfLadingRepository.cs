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
    public class MstBillOfLadingRepository : EfRepository<MstBillOfLading>, IMstBillOfLadingRepository
    {
        private ExpedicoEntities entities;

        public MstBillOfLadingRepository()
        { 
            entities = new ExpedicoEntities();
        }

        public IQueryable<MstBillOfLading> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public MstBillOfLading GetObjectById(int Id)
        {
            MstBillOfLading data = Find(x => x.Id == Id && !x.IsDeleted);
            if (data != null) { data.Errors = new Dictionary<string, string>(); }
            return data;
        }

        public MstBillOfLading CreateObject(MstBillOfLading model)
        {
            model.IsDeleted = false;
            model.CreatedAt = DateTime.Now;
            return Create(model);
        }

        public MstBillOfLading UpdateObject(MstBillOfLading model)
        {
            model.UpdatedAt = DateTime.Now;
            Update(model);
            return model;
        }

        public MstBillOfLading SoftDeleteObject(MstBillOfLading model)
        {
            model.IsDeleted = true;
            model.DeletedAt = DateTime.Now;
            Update(model);
            return model;
        }

        public bool DeleteObject(int Id)
        {
            MstBillOfLading data = Find(x => x.Id == Id);
            return (Delete(data) == 1) ? true : false;
        }
    }
}

