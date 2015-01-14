using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Validation
{
    public interface IPaymentRequestValidation
    {
        PaymentRequest VCreateObject(PaymentRequest paymentRequest, IShipmentOrderService _shipmentOrderService);
        PaymentRequest VUpdateObject(PaymentRequest paymentRequest, IPaymentRequestService _paymentRequestService, IShipmentOrderService _shipmentOrderService);
        PaymentRequest VSoftDeleteObject(PaymentRequest paymentRequest, IPaymentRequestService _paymentRequestService);
        PaymentRequest VConfirmObject(PaymentRequest paymentRequest, IPaymentRequestDetailService _paymentRequestDetailService);
        PaymentRequest VUnconfirmObject(PaymentRequest paymentRequest, IPayableService _payableService, IPaymentVoucherDetailService _paymentVoucherDetailService);
     }
}
