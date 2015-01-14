using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using Core.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validation.Validation
{ 
    public class PaymentRequestValidation : IPaymentRequestValidation
    {
        public PaymentRequest VCurrency(PaymentRequest paymentRequest)
        {
            if (paymentRequest.CurrencyId != MasterConstant.Currency.IDR && paymentRequest.CurrencyId != MasterConstant.Currency.USD)
            {
                paymentRequest.Errors.Add("CurrencyId", "Invalid Currency");
            }
            return paymentRequest;
        }

        public PaymentRequest VIsDeleted(PaymentRequest paymentRequest)
        {
            if (paymentRequest.IsDeleted == true)
            {
                paymentRequest.Errors.Add("Generic", "PaymentRequest Is Deleted");
            }
            return paymentRequest;
        }

        public PaymentRequest VIsUnConfirmed(PaymentRequest paymentRequest)
        {
            if (paymentRequest.IsConfirmed == false)
            {
                paymentRequest.Errors.Add("Generic", "PaymentRequest Is Not Confirmed");
            }
            return paymentRequest;
        }

        public PaymentRequest VIsConfirmed(PaymentRequest paymentRequest)
        {
            if (paymentRequest.IsConfirmed == true)
            {
                paymentRequest.Errors.Add("Generic", "PaymentRequest Is Confirmed");
            }
            return paymentRequest;
        }

        public PaymentRequest VvalidShipmentOrder(PaymentRequest paymentRequest, IShipmentOrderService _shipmentOrderService)
        {
            ShipmentOrder shipmentOrder = _shipmentOrderService.GetObjectById(paymentRequest.ShipmentOrderId);
            if (shipmentOrder == null)
            {
                paymentRequest.Errors.Add("ShipmentOrder", "Invalid ShipmentOrder");
            }
            else
            {
                if (shipmentOrder.OfficeId != paymentRequest.OfficeId)
                {
                    paymentRequest.Errors.Add("ShipmentOrder", "Invalid ShipmentOrder");
                }
            }
            return paymentRequest;
        }

        public PaymentRequest VvalidPaymentRequestUpdate(PaymentRequest paymentRequest, IPaymentRequestService _paymentRequestService)
        {
            PaymentRequest existPaymentRequest = _paymentRequestService.GetObjectById(paymentRequest.Id);
            if (existPaymentRequest != null)
            {
                paymentRequest.Errors.Add("Generic", "Invalid PaymentRequest");
            }
            else
            {
                if (existPaymentRequest.OfficeId != paymentRequest.OfficeId)
                {
                    paymentRequest.Errors.Add("Generic", "Invalid PaymentRequest");
                    return paymentRequest;
                }
                if (existPaymentRequest.Paid.HasValue && existPaymentRequest.Paid.Value == true)
                {
                    paymentRequest.Errors.Add("Generic", "PaymentRequest has been paid");
                    return paymentRequest;
                }
                if (existPaymentRequest.IsDeleted == true)
                {
                    paymentRequest.Errors.Add("Generic", "PaymentRequest has been deleted");
                    return paymentRequest;
                }
                if (existPaymentRequest.IsConfirmed == true)
                {
                    paymentRequest.Errors.Add("Generic", "PaymentRequest has been Confirmed");
                    return paymentRequest;
                }
            }
            return paymentRequest;
        }

        public PaymentRequest VvalidPaymentRequestDelete(PaymentRequest paymentRequest, IPaymentRequestService _paymentRequestService)
        {
            PaymentRequest existPaymentRequest = _paymentRequestService.GetObjectById(paymentRequest.Id);
            if (existPaymentRequest != null)
            {
                paymentRequest.Errors.Add("Generic", "Invalid PaymentRequest");
            }
            else
            {
                if (existPaymentRequest.OfficeId != paymentRequest.OfficeId)
                {
                    paymentRequest.Errors.Add("Generic", "Invalid PaymentRequest");
                    return paymentRequest;
                }
                if (existPaymentRequest.IsDeleted == true)
                {
                    paymentRequest.Errors.Add("Generic", "PaymentRequest has been deleted");
                    return paymentRequest;
                }
            }
            return paymentRequest;
        }

        public PaymentRequest VhasPaymentRequestDetail(PaymentRequest paymentRequest, IPaymentRequestDetailService _paymentRequestDetailService)
        {
            PaymentRequestDetail existPaymentRequestDetail = _paymentRequestDetailService.GetQueryable().Where(x => x.PaymentRequestId == paymentRequest.Id).FirstOrDefault();
            if (existPaymentRequestDetail == null)
            {
                paymentRequest.Errors.Add("Generic", "Tidak mempunyai detail");
            }
            return paymentRequest;
        }

        public PaymentRequest VvPaymentRequest(PaymentRequest paymentRequest, IPaymentRequestService _paymentRequestService)
        {
            PaymentRequest existPaymentRequest = _paymentRequestService.GetObjectById(paymentRequest.ShipmentOrderId);
            if (paymentRequest != null)
            {
                existPaymentRequest.Errors.Add("Generic", "Invalid PaymentRequest");
            }
            else
            {
                if (paymentRequest.OfficeId != existPaymentRequest.OfficeId)
                {
                    existPaymentRequest.Errors.Add("Generic", "Invalid PaymentRequest");
                }
            }
            return existPaymentRequest;
        }

        public PaymentRequest VPayableHasNoOtherAssociation(PaymentRequest paymentRequest, IPayableService _payableService, 
            IPaymentVoucherDetailService _paymentVoucherDetailService,IPaymentRequestDetailService _paymentRequestDetailService)
        {
            IList<PaymentRequestDetail> paymentRequestDetail = _paymentRequestDetailService.GetQueryable().Where(x => x.PaymentRequestId == paymentRequest.Id).ToList();
            foreach (var item in paymentRequestDetail)
            {
                Payable payable = _payableService.GetObjectBySource(MasterConstant.SourceDocument.Invoice, paymentRequest.Id,item.Id);
                PaymentVoucherDetail paymentVoucherDetail = _paymentVoucherDetailService.GetObjectsByPayableId(payable.Id).FirstOrDefault();
                if (paymentVoucherDetail != null)
                {
                    paymentRequest.Errors.Add("Generic", "Payment Request Sudah di Buat PaymentVoucheer : " + paymentVoucherDetail.PaymentVoucherId);
                }
            }
            return paymentRequest;
        }

        public PaymentRequest VCreateObject(PaymentRequest paymentRequest,IShipmentOrderService _shipmentOrderService)
        {
            VvalidShipmentOrder(paymentRequest, _shipmentOrderService);
            if (!isValid(paymentRequest)) { return paymentRequest; }
            VCurrency(paymentRequest);
            if (!isValid(paymentRequest)) { return paymentRequest; }
            return paymentRequest;
        }

        public PaymentRequest VUpdateObject(PaymentRequest paymentRequest, IPaymentRequestService _paymentRequestService, IShipmentOrderService _shipmentOrderService)
        {
            VvalidPaymentRequestUpdate(paymentRequest, _paymentRequestService);
            if (!isValid(paymentRequest)) { return paymentRequest; }
            VvPaymentRequest(paymentRequest, _paymentRequestService);
            if (!isValid(paymentRequest)) { return paymentRequest; }
            VvalidShipmentOrder(paymentRequest, _shipmentOrderService);
            if (!isValid(paymentRequest)) { return paymentRequest; }
            VCurrency(paymentRequest);
            if (!isValid(paymentRequest)) { return paymentRequest; }
            return paymentRequest;
        }

        public PaymentRequest VSoftDeleteObject(PaymentRequest paymentRequest, IPaymentRequestService _paymentRequestService)
        {
            VvalidPaymentRequestDelete(paymentRequest, _paymentRequestService);
            if (!isValid(paymentRequest)) { return paymentRequest; }
            return paymentRequest;
        }
         
        public PaymentRequest VConfirmObject(PaymentRequest paymentRequest, IPaymentRequestDetailService _paymentRequestDetailService)
        {
            VIsConfirmed(paymentRequest);
            if (!isValid(paymentRequest)) { return paymentRequest; }
            VIsDeleted(paymentRequest);
            if (!isValid(paymentRequest)) { return paymentRequest; }
            VhasPaymentRequestDetail(paymentRequest, _paymentRequestDetailService);
            VIsDeleted(paymentRequest);
            return paymentRequest;
        } 
         
        public PaymentRequest VUnconfirmObject(PaymentRequest paymentRequest,IPayableService _payableService,IPaymentVoucherDetailService _paymentVoucherDetailService)
        {
            VIsUnConfirmed(paymentRequest);
            if (!isValid(paymentRequest)) { return paymentRequest; }
            VIsDeleted(paymentRequest);
            if (!isValid(paymentRequest)) { return paymentRequest; }
           // VPayableHasNoOtherAssociation(paymentRequest, _payableService, _paymentVoucherDetailService);
           // if (!isValid(paymentRequest)) { return paymentRequest; }
            return paymentRequest;
        }

        public bool isValid(PaymentRequest obj)
        { 
            bool isValid = !obj.Errors.Any();
            return isValid;
        }
    }
}
