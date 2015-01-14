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
    public class TemporaryPaymentRepository : EfRepository<TemporaryPayment>, ITemporaryPaymentRepository 
    {
        private ExpedicoEntities entities;
         
        public TemporaryPaymentRepository()
        { 
            entities = new ExpedicoEntities();
        }

        public IQueryable<TemporaryPayment> GetQueryable()
        {
            return FindAll();
        }

        public TemporaryPayment GetObjectById(int Id)
        {
            TemporaryPayment data = Find(x => x.Id == Id && !x.IsDeleted);
            if (data != null) { data.Errors = new Dictionary<string, string>(); }
            return data;
        }

        public TemporaryPayment CreateObject(TemporaryPayment model)
        {
            model.IsDeleted = false;
            model.CreatedAt = DateTime.Now;
            return Create(model);
        }
         
        public int GetLastTPNo(int officeId)
        {
            TemporaryPayment data = FindAll(x => x.OfficeId == officeId).OrderByDescending(x => x.TPNo).FirstOrDefault();
            if (data != null)
            {
                return data.TPNo;
            }
            else
            {
                return 0;
            }
        } 

        public TemporaryPayment UpdateObject(TemporaryPayment model)
        {
            model.UpdatedAt = DateTime.Now;
            Update(model);
            return model;
        }

        public TemporaryPayment SoftDeleteObject(TemporaryPayment model)
        {
            model.IsDeleted = true;
            model.DeletedAt = DateTime.Now;
            Update(model);
            return model;
        }

        public bool DeleteObject(int Id)
        {
            TemporaryPayment data = Find(x => x.Id == Id);
            return (Delete(data) == 1) ? true : false;
        }

    }
}
