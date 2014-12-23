using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IPaymentRequestDetailService
    {
        IQueryable<PaymentRequestDetail> GetQueryable();
        PaymentRequestDetail GetObjectById(int Id);
        PaymentRequestDetail CreateObject(PaymentRequestDetail prDetail, IPaymentRequestService _paymentRequestService);
        PaymentRequestDetail UpdateObject(PaymentRequestDetail paymentRequestDetail, IPaymentRequestService _paymentRequestService);
        PaymentRequestDetail SoftDeleteObject(PaymentRequestDetail paymentRequestDetail, IPaymentRequestService _paymentRequestService);
        PaymentRequestDetail UnconfirmObject(PaymentRequestDetail model);
        PaymentRequestDetail ConfirmObject(PaymentRequestDetail model);
    }
}