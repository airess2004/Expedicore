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
    public class TemporaryReceiptRepository : EfRepository<TemporaryReceipt>, ITemporaryReceiptRepository 
    {
        private ExpedicoEntities entities;
         
        public TemporaryReceiptRepository()
        { 
            entities = new ExpedicoEntities();
        }

        public IQueryable<TemporaryReceipt> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public TemporaryReceipt GetObjectById(int Id)
        {
            TemporaryReceipt data = Find(x => x.Id == Id && !x.IsDeleted);
            if (data != null) { data.Errors = new Dictionary<string, string>(); }
            return data;
        }

        public TemporaryReceipt CreateObject(TemporaryReceipt model)
        {
            model.IsDeleted = false;
            model.CreatedAt = DateTime.Now;
            return Create(model);
        }
         
        public int GetLastTRNo(int officeId)
        {
            TemporaryReceipt data = FindAll(x => x.OfficeId == officeId).OrderByDescending(x => x.TemporaryReceiptNo).FirstOrDefault();
            if (data != null)
            {
                return data.TemporaryReceiptNo;
            }
            else
            {
                return 0;
            }
        } 

        public TemporaryReceipt UpdateObject(TemporaryReceipt model)
        {
            model.UpdatedAt = DateTime.Now;
            Update(model);
            return model;
        }

        public TemporaryReceipt SoftDeleteObject(TemporaryReceipt model)
        {
            model.IsDeleted = true;
            model.DeletedAt = DateTime.Now;
            Update(model);
            return model;
        }

        public bool DeleteObject(int Id)
        {
            TemporaryReceipt data = Find(x => x.Id == Id);
            return (Delete(data) == 1) ? true : false;
        }

    }
}
