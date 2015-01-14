using Core.Constants;
using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service
{
    public class PaymentVoucherService : IPaymentVoucherService
    {
        private IPaymentVoucherRepository _repository;
        private IPaymentVoucherValidator _validator;

        public PaymentVoucherService(IPaymentVoucherRepository _paymentVoucherRepository, IPaymentVoucherValidator _paymentVoucherValidator)
        {
            _repository = _paymentVoucherRepository;
            _validator = _paymentVoucherValidator;
        }

        public IPaymentVoucherValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<PaymentVoucher> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<PaymentVoucher> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<PaymentVoucher> GetObjectsByCashBankId(int cashBankId)
        {
            return _repository.GetObjectsByCashBankId(cashBankId);
        }

        public PaymentVoucher GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public IList<PaymentVoucher> GetObjectsByContactId(int contactId)
        {
            return _repository.GetObjectsByContactId(contactId);
        }

        public PaymentVoucher CreateObject(PaymentVoucher paymentVoucher, IPaymentVoucherDetailService _paymentVoucherDetailService,
                                            IPayableService _payableService, IContactService _contactService, 
                                            ICashBankService _cashBankService)
        {
            paymentVoucher.Errors = new Dictionary<String, String>();
            paymentVoucher.PaymentDate = DateTime.Today;
            paymentVoucher.IsGBCH = false;

            CashBank cashBank = _cashBankService.GetObjectById(paymentVoucher.CashBankId);
            return (_validator.ValidCreateObject(paymentVoucher, this, _paymentVoucherDetailService, _payableService, _contactService, _cashBankService) ?
                    _repository.CreateObject(paymentVoucher) : paymentVoucher);
        }

        public PaymentVoucher UpdateObject(PaymentVoucher paymentVoucher, IPaymentVoucherDetailService _paymentVoucherDetailService, IPayableService _payableService, IContactService _contactService, ICashBankService _cashBankService)
        {
            return (_validator.ValidUpdateObject(paymentVoucher, this, _paymentVoucherDetailService, _payableService, _contactService, _cashBankService) ? _repository.UpdateObject(paymentVoucher) : paymentVoucher);
        }

        public PaymentVoucher UpdateAmount(PaymentVoucher paymentVoucher)
        {
            return _repository.UpdateObject(paymentVoucher);
        }

        public PaymentVoucher SoftDeleteObject(PaymentVoucher paymentVoucher, IPaymentVoucherDetailService _paymentVoucherDetailService)
        {
            return (_validator.ValidDeleteObject(paymentVoucher, _paymentVoucherDetailService) ? _repository.SoftDeleteObject(paymentVoucher) : paymentVoucher);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public PaymentVoucher CalculateTotalAmount(PaymentVoucher paymentVoucher, IPaymentVoucherDetailService _paymentVoucherDetailService)
        {
            IList<PaymentVoucherDetail> paymentVoucherDetails = _paymentVoucherDetailService.GetObjectsByPaymentVoucherId(paymentVoucher.Id);
            decimal totalIDR = 0;
            decimal totalUSD = 0;

            foreach (PaymentVoucherDetail detail in paymentVoucherDetails)
            {
                totalIDR += detail.AmountIDR;
                totalUSD += detail.AmountUSD;
            }
            paymentVoucher.TotalAmountIDR = totalIDR;
            paymentVoucher.TotalAmountUSD = totalUSD;
            paymentVoucher = _repository.UpdateObject(paymentVoucher);
            return paymentVoucher;
        }

        public PaymentVoucher ConfirmObject(PaymentVoucher paymentVoucher, DateTime ConfirmationDate, IPaymentVoucherDetailService _paymentVoucherDetailService,
                                            ICashBankService _cashBankService, IPayableService _payableService, ICashMutationService _cashMutationService,IPaymentRequestService _paymentRequestService)
        {
            paymentVoucher.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(paymentVoucher, this, _paymentVoucherDetailService, _cashBankService, _payableService))
            {
                IList<PaymentVoucherDetail> details = _paymentVoucherDetailService.GetObjectsByPaymentVoucherId(paymentVoucher.Id);
                foreach (var detail in details)
                {
                    detail.Errors = new Dictionary<string, string>();
                    _paymentVoucherDetailService.ConfirmObject(detail, ConfirmationDate, this, _payableService,_paymentRequestService);
                }
                ///
                _repository.ConfirmObject(paymentVoucher);
                CashBank cashBank = _cashBankService.GetObjectById(paymentVoucher.CashBankId);

                if (!paymentVoucher.IsGBCH)
                {
                    CashMutation cashMutation = _cashMutationService.CreateCashMutationForPaymentVoucher(paymentVoucher, cashBank);
                    _cashMutationService.CashMutateObject(cashMutation, _cashBankService);
                }
            }
            return paymentVoucher;
        }

        public PaymentVoucher UnconfirmObject(PaymentVoucher paymentVoucher, IPaymentVoucherDetailService _paymentVoucherDetailService,
                                            ICashBankService _cashBankService, IPayableService _payableService, ICashMutationService _cashMutationService,IPaymentRequestService _paymentRequestService)
        {
            if (_validator.ValidUnconfirmObject(paymentVoucher))
            {
                IList<PaymentVoucherDetail> details = _paymentVoucherDetailService.GetObjectsByPaymentVoucherId(paymentVoucher.Id);
                foreach (var detail in details)
                {
                    detail.Errors = new Dictionary<string, string>();
                    _paymentVoucherDetailService.UnconfirmObject(detail, this, _payableService,_paymentRequestService);
                }
                CashBank cashBank = _cashBankService.GetObjectById(paymentVoucher.CashBankId);
                if (!paymentVoucher.IsGBCH)
                {
                    IList<CashMutation> cashMutations = _cashMutationService.SoftDeleteCashMutationForPaymentVoucher(paymentVoucher, cashBank);
                    foreach (var cashMutation in cashMutations)
                    {
                        _cashMutationService.ReverseCashMutateObject(cashMutation, _cashBankService);
                    }
                }
                _repository.UnconfirmObject(paymentVoucher);

            }
            return paymentVoucher;
        }

        public PaymentVoucher ReconcileObject(PaymentVoucher paymentVoucher, DateTime ReconciliationDate, IPaymentVoucherDetailService _paymentVoucherDetailService,
                                              ICashMutationService _cashMutationService, ICashBankService _cashBankService, IPayableService _payableService)
        {
            paymentVoucher.ReconciliationDate = ReconciliationDate;
            if (_validator.ValidReconcileObject(paymentVoucher, _paymentVoucherDetailService, _cashBankService))
            {
                CashBank cashBank = _cashBankService.GetObjectById(paymentVoucher.CashBankId);
                CashMutation cashMutation = _cashMutationService.CreateCashMutationForPaymentVoucher(paymentVoucher, cashBank);
                _repository.ReconcileObject(paymentVoucher);

                _cashMutationService.CashMutateObject(cashMutation, _cashBankService);

                IList<PaymentVoucherDetail> paymentVoucherDetails = _paymentVoucherDetailService.GetObjectsByPaymentVoucherId(paymentVoucher.Id);
                foreach(var paymentVoucherDetail in paymentVoucherDetails)
                {
                    Payable payable = _payableService.GetObjectById(paymentVoucherDetail.PayableId);
                    payable.PendingClearanceAmount -= paymentVoucherDetail.AmountUSD + paymentVoucherDetail.AmountIDR;
                    if (payable.PendingClearanceAmount == 0 && payable.RemainingAmount == 0)
                    {
                        payable.IsCompleted = true;
                        payable.CompletionDate = DateTime.Now;
                    }
                    _payableService.UpdateObject(payable);
                }
            }
            return paymentVoucher;
        }

        public PaymentVoucher UnreconcileObject(PaymentVoucher paymentVoucher, IPaymentVoucherDetailService _paymentVoucherDetailService,
                                                ICashMutationService _cashMutationService, ICashBankService _cashBankService, IPayableService _payableService)
        {
            if (_validator.ValidUnreconcileObject(paymentVoucher))
            {
                CashBank cashBank = _cashBankService.GetObjectById(paymentVoucher.CashBankId);
                _repository.UnreconcileObject(paymentVoucher);

                IList<CashMutation> cashMutations = _cashMutationService.SoftDeleteCashMutationForPaymentVoucher(paymentVoucher, cashBank);
                foreach (var cashMutation in cashMutations)
                {
                    _cashMutationService.ReverseCashMutateObject(cashMutation, _cashBankService);
                }

                IList<PaymentVoucherDetail> paymentVoucherDetails = _paymentVoucherDetailService.GetObjectsByPaymentVoucherId(paymentVoucher.Id);
                foreach (var paymentVoucherDetail in paymentVoucherDetails)
                {
                    Payable payable = _payableService.GetObjectById(paymentVoucherDetail.PayableId);
                    payable.PendingClearanceAmount += paymentVoucherDetail.AmountUSD + paymentVoucherDetail.AmountIDR;
                    if (payable.PendingClearanceAmount != 0 || payable.RemainingAmount != 0)
                    {
                        payable.IsCompleted = false;
                        payable.CompletionDate = null;
                    }
                    _payableService.UpdateObject(payable);
                }
            }
            return paymentVoucher;
        }
    }
}