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
    public class PaymentRequestRepository : EfRepository<PaymentRequest>, IPaymentRequestRepository 
    {
        private ExpedicoEntities entities;
         
        public PaymentRequestRepository()
        { 
            entities = new ExpedicoEntities();
        }

        public IQueryable<PaymentRequest> GetQueryable()
        {
            return FindAll();
        }

        public PaymentRequest GetObjectByShipmentOrderId(int Id)
        {
            PaymentRequest data = Find(x => x.ShipmentOrderId == Id && !x.IsDeleted);
            if (data != null) { data.Errors = new Dictionary<string, string>(); }
            return data;
        }

        public PaymentRequest GetObjectById(int Id)
        {
            PaymentRequest data = Find(x => x.Id == Id && !x.IsDeleted);
            if (data != null) { data.Errors = new Dictionary<string, string>(); }
            return data;
        }


        public int GetLastPRNo(int officeId,string debetCredit)
        {
            PaymentRequest data = FindAll(x => x.OfficeId == officeId && x.DebetCredit == debetCredit).OrderByDescending(x => x.PRNo).FirstOrDefault();
            if (data != null)
            {
                return data.PRNo;
            }
            else
            {
                return 0;
            }
        } 

        public int GetLastPRStatus(int officeId, int shipmentId)
        {
            PaymentRequest data = FindAll(b => b.OfficeId == officeId && b.ShipmentOrderId == shipmentId).OrderByDescending(x => x.PRStatus).FirstOrDefault();
            if (data != null)
            {
                return data.PRStatus;
            }
            else
            {
                return 0;
            }
        }

        public PaymentRequest CreateObject(PaymentRequest model)
        {
            model.IsDeleted = false;
            model.CreatedAt = DateTime.Now;
            return Create(model);
        }

        public PaymentRequest UpdateObject(PaymentRequest model)
        {
            model.UpdatedAt = DateTime.Now;
            Update(model);
            return model;
        }

        public PaymentRequest SoftDeleteObject(PaymentRequest model)
        {
            model.IsDeleted = true;
            model.DeletedAt = DateTime.Now;
            Update(model);
            return model;
        }

        public bool DeleteObject(int Id)
        {
            PaymentRequest data = Find(x => x.Id == Id);
            return (Delete(data) == 1) ? true : false;
        }

        public PaymentRequest ConfirmObject(PaymentRequest model)
        {
            model.IsConfirmed = true;
            Update(model);
            return model;
        }

        public PaymentRequest UnconfirmObject(PaymentRequest model)
        {
            model.ConfirmationDate = null;
            model.IsConfirmed = false;
            Update(model);
            return model;
        }
    }
}
