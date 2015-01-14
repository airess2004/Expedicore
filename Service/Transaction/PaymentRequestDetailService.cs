using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;
using Core.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class PaymentRequestDetailService : IPaymentRequestDetailService 
    {  
        private IPaymentRequestDetailRepository _repository;
        private IPaymentRequestDetailValidation _validator;

        public PaymentRequestDetailService(IPaymentRequestDetailRepository _paymentRequestDetailRepository, IPaymentRequestDetailValidation _paymentRequestDetailValidation)
        {
            _repository = _paymentRequestDetailRepository;
            _validator = _paymentRequestDetailValidation;
        }

        public IQueryable<PaymentRequestDetail> GetQueryable()
        {  
            return _repository.GetQueryable();
        }

        public PaymentRequestDetail GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public PaymentRequestDetail CreateObject(PaymentRequestDetail prDetail, IPaymentRequestService _paymentRequestService)
        {
            prDetail.Errors = new Dictionary<String, String>();
            if (isValid(_validator.VCreateObject(prDetail,_paymentRequestService)))
            {
                PaymentRequestDetail newPRDetail = new PaymentRequestDetail();
                newPRDetail.Errors = new Dictionary<string, string>();
                newPRDetail.CostId = prDetail.CostId;
                newPRDetail.Amount = prDetail.Amount;
                newPRDetail.AmountCrr = prDetail.AmountCrr;
                newPRDetail.CodingQuantity = prDetail.CodingQuantity;
                newPRDetail.OfficeId = prDetail.OfficeId;
                newPRDetail.CreatedById = prDetail.CreatedById;
                newPRDetail.CreatedAt = DateTime.Today;
                newPRDetail.DebetCredit = MasterConstant.DebetCredit.Credit;
                newPRDetail.Description = !String.IsNullOrEmpty(prDetail.Description) ? prDetail.Description.ToUpper() : "";
                newPRDetail.PerQty = prDetail.PerQty;
                newPRDetail.PaymentRequestId = prDetail.PaymentRequestId;
                newPRDetail.Quantity = prDetail.Quantity;
                newPRDetail.Type = prDetail.Type;
                newPRDetail.EPLDetailId = prDetail.EPLDetailId;
                prDetail = _repository.CreateObject(newPRDetail);

                PaymentRequest paymentRequest = _paymentRequestService.GetObjectById(newPRDetail.PaymentRequestId);
                _paymentRequestService.CalculateTotalPaymentRequest(paymentRequest, this);
            }

            return prDetail;
        }

        public PaymentRequestDetail ConfirmObject(PaymentRequestDetail paymentRequestDetail)
        {
            paymentRequestDetail = _repository.ConfirmObject(paymentRequestDetail);
            return paymentRequestDetail;
        }

        public PaymentRequestDetail UnconfirmObject(PaymentRequestDetail paymentRequestDetail)
        {
            paymentRequestDetail = _repository.UnconfirmObject(paymentRequestDetail);
            return paymentRequestDetail;
        }

        public PaymentRequestDetail UpdateObject(PaymentRequestDetail paymentRequestDetail,IPaymentRequestService _paymentRequestService)
        {
            if (isValid(_validator.VUpdateObject(paymentRequestDetail,_paymentRequestService,this)))
            {
                paymentRequestDetail = _repository.UpdateObject(paymentRequestDetail);
                PaymentRequest paymentRequest = _paymentRequestService.GetObjectById(paymentRequestDetail.PaymentRequestId);
                _paymentRequestService.CalculateTotalPaymentRequest(paymentRequest, this);
            }
            return paymentRequestDetail;
        }

        public PaymentRequestDetail SoftDeleteObject(PaymentRequestDetail paymentRequestDetail,IPaymentRequestService _paymentRequestService)
        {
            if (isValid(_validator.VSoftDeleteObject(paymentRequestDetail, _paymentRequestService, this)))
            {
                paymentRequestDetail = _repository.SoftDeleteObject(paymentRequestDetail);
                PaymentRequest paymentRequest = _paymentRequestService.GetObjectById(paymentRequestDetail.PaymentRequestId);
                _paymentRequestService.CalculateTotalPaymentRequest(paymentRequest, this);
            }
            return paymentRequestDetail;
        }

        public bool isValid(PaymentRequestDetail obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }
    }
}
