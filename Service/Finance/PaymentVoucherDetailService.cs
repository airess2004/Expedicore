using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;
using Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service
{
    public class PaymentVoucherDetailService : IPaymentVoucherDetailService
    {
        private IPaymentVoucherDetailRepository _repository;
        private IPaymentVoucherDetailValidator _validator;

        public PaymentVoucherDetailService(IPaymentVoucherDetailRepository _paymentVoucherDetailRepository, IPaymentVoucherDetailValidator _paymentVoucherDetailValidator)
        {
            _repository = _paymentVoucherDetailRepository;
            _validator = _paymentVoucherDetailValidator;
        }

        public IPaymentVoucherDetailValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<PaymentVoucherDetail> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<PaymentVoucherDetail> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<PaymentVoucherDetail> GetObjectsByPaymentVoucherId(int paymentVoucherId)
        {
            return _repository.GetObjectsByPaymentVoucherId(paymentVoucherId);
        }

        public IList<PaymentVoucherDetail> GetObjectsByPayableId(int payableId)
        {
            return _repository.GetObjectsByPayableId(payableId);
        }

        public PaymentVoucherDetail GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public PaymentVoucherDetail CreateObject(PaymentVoucherDetail paymentVoucherDetail, IPaymentVoucherService _paymentVoucherService,
                                                ICashBankService _cashBankService, IPayableService _payableService)
        {
            paymentVoucherDetail.Errors = new Dictionary<String, String>();
            if (_validator.ValidCreateObject(paymentVoucherDetail, _paymentVoucherService, this, _cashBankService, _payableService))
            {
                paymentVoucherDetail = _repository.CreateObject(paymentVoucherDetail);
                PaymentVoucher paymentVoucher = _paymentVoucherService.GetObjectById(paymentVoucherDetail.PaymentVoucherId);
                _paymentVoucherService.CalculateTotalAmount(paymentVoucher, this);
            }
            return paymentVoucherDetail;
        }

        public PaymentVoucherDetail CreateObject(int paymentVoucherId, int payableId, decimal amount, string description, 
                                         IPaymentVoucherService _paymentVoucherService, ICashBankService _cashBankService,
                                         IPayableService _payableService)
        {
            PaymentVoucherDetail paymentVoucherDetail = new PaymentVoucherDetail
            {
                PaymentVoucherId = paymentVoucherId,
                PayableId = payableId,
                AmountUSD = amount,
                Description = description,
            };
            return this.CreateObject(paymentVoucherDetail, _paymentVoucherService, _cashBankService, _payableService);
        }

        public PaymentVoucherDetail UpdateObject(PaymentVoucherDetail paymentVoucherDetail, IPaymentVoucherService _paymentVoucherService, ICashBankService _cashBankService, IPayableService _payableService)
        {
            if (_validator.ValidUpdateObject(paymentVoucherDetail, _paymentVoucherService, this, _cashBankService, _payableService))
            {
                paymentVoucherDetail = _repository.UpdateObject(paymentVoucherDetail);
                PaymentVoucher paymentVoucher = _paymentVoucherService.GetObjectById(paymentVoucherDetail.PaymentVoucherId);
                _paymentVoucherService.CalculateTotalAmount(paymentVoucher, this);
            }
            return paymentVoucherDetail;
        }

        public PaymentVoucherDetail SoftDeleteObject(PaymentVoucherDetail paymentVoucherDetail,IPaymentVoucherService _paymentVoucherService)
        {
            if (_validator.ValidDeleteObject(paymentVoucherDetail))
            {
                paymentVoucherDetail = _repository.SoftDeleteObject(paymentVoucherDetail);
                PaymentVoucher paymentVoucher = _paymentVoucherService.GetObjectById(paymentVoucherDetail.PaymentVoucherId);
                _paymentVoucherService.CalculateTotalAmount(paymentVoucher, this);
            }
            return paymentVoucherDetail;
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public PaymentVoucherDetail ConfirmObject(PaymentVoucherDetail paymentVoucherDetail, DateTime ConfirmationDate,
                                                  IPaymentVoucherService _paymentVoucherService, IPayableService _payableService,IPaymentRequestService _paymentRequestService)
        {
            paymentVoucherDetail.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(paymentVoucherDetail, _payableService))
            {
                PaymentVoucher paymentVoucher = _paymentVoucherService.GetObjectById(paymentVoucherDetail.PaymentVoucherId);
                Payable payable = _payableService.GetObjectById(paymentVoucherDetail.PayableId);

                if (paymentVoucher.IsGBCH) { payable.PendingClearanceAmount += paymentVoucherDetail.AmountUSD + paymentVoucherDetail.AmountIDR; }
                payable.RemainingAmount -= paymentVoucherDetail.AmountUSD + paymentVoucherDetail.AmountIDR;
                if (payable.RemainingAmount == 0 && payable.PendingClearanceAmount == 0)
                {
                    payable.IsCompleted = true;
                    payable.CompletionDate = DateTime.Now;
                }
                _payableService.UpdateObject(payable);

                if (_payableService.GetQueryable().Where(x => x.PayableSourceId == payable.PayableSourceId
                   && x.IsCompleted == true && x.IsDeleted == false).Count() ==
                   _payableService.GetQueryable().Where(x => x.PayableSourceId == payable.PayableSourceId
                   && x.IsDeleted == false).Count())
                {
                    PaymentRequest paymentRequest = _paymentRequestService.GetObjectById(payable.PayableSourceId);
                    _paymentRequestService.Paid(paymentRequest);
                }


                paymentVoucherDetail = _repository.ConfirmObject(paymentVoucherDetail);
            }
            return paymentVoucherDetail;
        }

        public PaymentVoucherDetail UnconfirmObject(PaymentVoucherDetail paymentVoucherDetail, IPaymentVoucherService _paymentVoucherService, IPayableService _payableService,IPaymentRequestService _paymentRequestService)
        {
            if (_validator.ValidUnconfirmObject(paymentVoucherDetail))
            {
                PaymentVoucher paymentVoucher = _paymentVoucherService.GetObjectById(paymentVoucherDetail.PaymentVoucherId);
                Payable payable = _payableService.GetObjectById(paymentVoucherDetail.PayableId);

                if (paymentVoucher.IsGBCH) { payable.PendingClearanceAmount -= paymentVoucherDetail.AmountUSD + paymentVoucherDetail.AmountIDR; }
                payable.RemainingAmount += paymentVoucherDetail.AmountUSD + paymentVoucherDetail.AmountIDR;
                if (payable.RemainingAmount != 0 || payable.PendingClearanceAmount != 0)
                {
                    payable.IsCompleted = false;
                    payable.CompletionDate = null;
                }
                _payableService.UpdateObject(payable);

                if (_payableService.GetQueryable().Where(x => x.PayableSourceId == payable.PayableSourceId
                && x.IsCompleted == false && x.IsDeleted == false).FirstOrDefault() != null)
                {
                    PaymentRequest paymentRequest = _paymentRequestService.GetObjectById(payable.PayableSourceId);
                    _paymentRequestService.Unpaid(paymentRequest);
                }


                paymentVoucherDetail = _repository.UnconfirmObject(paymentVoucherDetail);
            }
            return paymentVoucherDetail;
        }
    }
}