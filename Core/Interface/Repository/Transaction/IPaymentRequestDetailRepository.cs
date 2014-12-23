using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IPaymentRequestDetailRepository : IRepository<PaymentRequestDetail>
    { 
       IQueryable<PaymentRequestDetail> GetQueryable();
       PaymentRequestDetail GetObjectById(int Id);
       PaymentRequestDetail CreateObject(PaymentRequestDetail model);
       PaymentRequestDetail UpdateObject(PaymentRequestDetail model);
       PaymentRequestDetail SoftDeleteObject(PaymentRequestDetail model);
       PaymentRequestDetail UnconfirmObject(PaymentRequestDetail model);
       PaymentRequestDetail ConfirmObject(PaymentRequestDetail model);
       bool DeleteObject(int Id);
    }
}