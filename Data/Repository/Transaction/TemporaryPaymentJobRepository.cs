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
    public class TemporaryPaymentJobRepository : EfRepository<TemporaryPaymentJob>, ITemporaryPaymentJobRepository 
    {
        private ExpedicoEntities entities;
         
        public TemporaryPaymentJobRepository()
        { 
            entities = new ExpedicoEntities();
        }

        public IQueryable<TemporaryPaymentJob> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public TemporaryPaymentJob GetObjectById(int Id)
        {
            TemporaryPaymentJob data = Find(x => x.Id == Id && !x.IsDeleted);
            if (data != null) { data.Errors = new Dictionary<string, string>(); }
            return data;
        }

        public TemporaryPaymentJob CreateObject(TemporaryPaymentJob model)
        {
            model.IsDeleted = false;
            model.CreatedAt = DateTime.Now;
            return Create(model);
        }
         

        public TemporaryPaymentJob UpdateObject(TemporaryPaymentJob model)
        {
            model.UpdatedAt = DateTime.Now;
            Update(model);
            return model;
        }

        public TemporaryPaymentJob SoftDeleteObject(TemporaryPaymentJob model)
        {
            model.IsDeleted = true;
            model.DeletedAt = DateTime.Now;
            Update(model);
            return model;
        }

        public bool DeleteObject(int Id)
        {
            TemporaryPaymentJob data = Find(x => x.Id == Id);
            return (Delete(data) == 1) ? true : false;
        }

    }
}
