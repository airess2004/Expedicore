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
    public class PaymentRequestDetailRepository : EfRepository<PaymentRequestDetail>, IPaymentRequestDetailRepository 
    {
        private ExpedicoEntities entities;
         
        public PaymentRequestDetailRepository()
        { 
            entities = new ExpedicoEntities();
        }

        public IQueryable<PaymentRequestDetail> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public PaymentRequestDetail GetObjectById(int Id)
        {
            PaymentRequestDetail data = Find(x => x.Id == Id && !x.IsDeleted);
            if (data != null) { data.Errors = new Dictionary<string, string>(); }
            return data;
        }

        public PaymentRequestDetail CreateObject(PaymentRequestDetail model)
        {
            model.IsDeleted = false;
            model.CreatedAt = DateTime.Now;
            return Create(model);
        }

        public PaymentRequestDetail UpdateObject(PaymentRequestDetail model)
        {
            model.UpdatedAt = DateTime.Now;
            Update(model);
            return model;
        }

        public PaymentRequestDetail SoftDeleteObject(PaymentRequestDetail model)
        {
            model.IsDeleted = true;
            model.DeletedAt = DateTime.Now;
            Update(model);
            return model;
        }

        public bool DeleteObject(int Id)
        {
            PaymentRequestDetail data = Find(x => x.Id == Id);
            return (Delete(data) == 1) ? true : false;
        }

        public PaymentRequestDetail ConfirmObject(PaymentRequestDetail model)
        {
            model.IsConfirmed = true;
            Update(model);
            return model;
        }

        public PaymentRequestDetail UnconfirmObject(PaymentRequestDetail model)
        {
            model.ConfirmationDate = null;
            model.IsConfirmed = false;
            Update(model);
            return model;
        }

    }
}
