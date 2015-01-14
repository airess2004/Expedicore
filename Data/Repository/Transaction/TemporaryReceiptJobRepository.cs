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
    public class TemporaryReceiptJobRepository : EfRepository<TemporaryReceiptJob>, ITemporaryReceiptJobRepository 
    {
        private ExpedicoEntities entities;
         
        public TemporaryReceiptJobRepository()
        { 
            entities = new ExpedicoEntities();
        }

        public IQueryable<TemporaryReceiptJob> GetQueryable()
        {
            return FindAll();
        }

        public TemporaryReceiptJob GetObjectById(int Id)
        {
            TemporaryReceiptJob data = Find(x => x.Id == Id && !x.IsDeleted);
            if (data != null) { data.Errors = new Dictionary<string, string>(); }
            return data;
        }

        public TemporaryReceiptJob CreateObject(TemporaryReceiptJob model)
        {
            model.IsDeleted = false;
            model.CreatedAt = DateTime.Now;
            return Create(model);
        }
         

        public TemporaryReceiptJob UpdateObject(TemporaryReceiptJob model)
        {
            model.UpdatedAt = DateTime.Now;
            Update(model);
            return model;
        }

        public TemporaryReceiptJob SoftDeleteObject(TemporaryReceiptJob model)
        {
            model.IsDeleted = true;
            model.DeletedAt = DateTime.Now;
            Update(model);
            return model;
        }

        public bool DeleteObject(int Id)
        {
            TemporaryReceiptJob data = Find(x => x.Id == Id);
            return (Delete(data) == 1) ? true : false;
        }

    }
}
