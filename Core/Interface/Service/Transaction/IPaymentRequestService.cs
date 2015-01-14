using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IPaymentRequestService
    {
        IQueryable<PaymentRequest> GetQueryable();
        PaymentRequest GetObjectById(int Id);
        PaymentRequest GetObjectByShipmentOrderId(int Id);
        PaymentRequest CreateObject(PaymentRequest paymentRequest, IShipmentOrderService _shipmentOrderService);
        PaymentRequest UpdateObject(PaymentRequest paymentRequest, IShipmentOrderService _shipmentOrderService);
        PaymentRequest SoftDeleteObject(PaymentRequest paymentRequest);
        PaymentRequest ConfirmObject(PaymentRequest paymentRequest, DateTime confirmationDate, IPaymentRequestDetailService _paymentRequestDetailService, IPayableService _payableService);
        PaymentRequest UnconfirmObject(PaymentRequest paymentRequest, IPaymentRequestDetailService _paymentRequestDetailService, IPayableService _payableService, IPaymentVoucherDetailService _paymentVoucherDetailService);
        PaymentRequest CalculateTotalPaymentRequest(PaymentRequest paymentRequest, IPaymentRequestDetailService _paymentRequestDetailService);
        PaymentRequest Paid(PaymentRequest paymentRequest);
        PaymentRequest Unpaid(PaymentRequest paymentRequest);
    }
}