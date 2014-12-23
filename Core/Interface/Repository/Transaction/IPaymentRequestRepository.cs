using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IPaymentRequestRepository : IRepository<PaymentRequest>
    { 
       IQueryable<PaymentRequest> GetQueryable();
       PaymentRequest GetObjectById(int Id);
       PaymentRequest GetObjectByShipmentOrderId(int Id);
       PaymentRequest CreateObject(PaymentRequest model);
       PaymentRequest UpdateObject(PaymentRequest model);
       PaymentRequest SoftDeleteObject(PaymentRequest model);
       int GetLastPRNo(int officeId, string debetCredit);
       int GetLastPRStatus(int officeId, int shipmentId);
       PaymentRequest UnconfirmObject(PaymentRequest model);
       PaymentRequest ConfirmObject(PaymentRequest model);
       bool DeleteObject(int Id);
    }
}