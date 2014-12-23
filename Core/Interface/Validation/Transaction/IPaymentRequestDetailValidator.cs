using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Validation
{
    public interface IPaymentRequestDetailValidation
    {
        PaymentRequestDetail VCreateObject(PaymentRequestDetail paymentRequestDetail, IPaymentRequestService _paymentRequestService);
        PaymentRequestDetail VUpdateObject(PaymentRequestDetail paymentRequestDetail, IPaymentRequestService _paymentRequestService, IPaymentRequestDetailService _paymentRequestDetailService);
        PaymentRequestDetail VSoftDeleteObject(PaymentRequestDetail paymentRequestDetail, IPaymentRequestService _paymentRequestService, IPaymentRequestDetailService _paymentRequestDetailService);
    }
}
