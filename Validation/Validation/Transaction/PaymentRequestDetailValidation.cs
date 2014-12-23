using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validation.Validation
{ 
    public class PaymentRequestDetailValidation : IPaymentRequestDetailValidation
    {
        public PaymentRequestDetail VvalidPaymentRequest(PaymentRequestDetail paymentRequestDetail, IPaymentRequestService _paymentRequestService)
        {
            PaymentRequest existPaymentRequest = _paymentRequestService.GetObjectById(paymentRequestDetail.PaymentRequestId);
            if (existPaymentRequest != null)
            {
                paymentRequestDetail.Errors.Add("Generic", "Invalid PaymentRequest");
            }
            else
            {
                if (existPaymentRequest.OfficeId != paymentRequestDetail.OfficeId)
                {
                    paymentRequestDetail.Errors.Add("Generic", "Invalid PaymentRequest");
                    return paymentRequestDetail;
                }
                if (existPaymentRequest.Paid.HasValue && existPaymentRequest.Paid.Value == true)
                {
                    paymentRequestDetail.Errors.Add("Generic", "PaymentRequest has been paid");
                    return paymentRequestDetail;
                }
                if (existPaymentRequest.IsDeleted == true)
                {
                    paymentRequestDetail.Errors.Add("Generic", "PaymentRequest has been deleted");
                    return paymentRequestDetail;
                }
                if (existPaymentRequest.IsConfirmed == true)
                {
                    paymentRequestDetail.Errors.Add("Generic", "PaymentRequest has been Confirmed");
                    return paymentRequestDetail;
                }
            }
            return paymentRequestDetail;
        }

        public PaymentRequestDetail VValidPaymentRequestDetail(PaymentRequestDetail paymentRequestDetail, IPaymentRequestDetailService _paymentRequestDetailService)
        {
            PaymentRequestDetail existPaymentRequestDetail = _paymentRequestDetailService.GetObjectById(paymentRequestDetail.Id);
            if (existPaymentRequestDetail == null)
            {
                paymentRequestDetail.Errors.Add("Generic", "Invalid PaymentRequest Detail");
                return paymentRequestDetail;
            }
            return paymentRequestDetail;
        }


        public PaymentRequestDetail VCreateObject(PaymentRequestDetail paymentRequestDetail, IPaymentRequestService _paymentRequestService)
        {
            VvalidPaymentRequest(paymentRequestDetail, _paymentRequestService);
            if (!isValid(paymentRequestDetail)) { return paymentRequestDetail; }
            return paymentRequestDetail;
        }

        public PaymentRequestDetail VUpdateObject(PaymentRequestDetail paymentRequestDetail, IPaymentRequestService _paymentRequestService, IPaymentRequestDetailService _paymentRequestDetailService)
        {
            VvalidPaymentRequest(paymentRequestDetail, _paymentRequestService);
            if (!isValid(paymentRequestDetail)) { return paymentRequestDetail; }
            VValidPaymentRequestDetail(paymentRequestDetail, _paymentRequestDetailService);
            if (!isValid(paymentRequestDetail)) { return paymentRequestDetail; }
            return paymentRequestDetail;
        }

        public PaymentRequestDetail VSoftDeleteObject(PaymentRequestDetail paymentRequestDetail, IPaymentRequestService _paymentRequestService, IPaymentRequestDetailService _paymentRequestDetailService)
        {
            VvalidPaymentRequest(paymentRequestDetail, _paymentRequestService);
            if (!isValid(paymentRequestDetail)) { return paymentRequestDetail; }
            VValidPaymentRequestDetail(paymentRequestDetail, _paymentRequestDetailService);
            if (!isValid(paymentRequestDetail)) { return paymentRequestDetail; }
            return paymentRequestDetail;
        }

        public bool isValid(PaymentRequestDetail obj)
        { 
            bool isValid = !obj.Errors.Any();
            return isValid;
        }
    }
}
