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
    public class PaymentRequestService : IPaymentRequestService 
    {  
        private IPaymentRequestRepository _repository;
        private IPaymentRequestValidation _validator;

        public PaymentRequestService(IPaymentRequestRepository _paymentRequestRepository, IPaymentRequestValidation _paymentRequestValidation)
        {
            _repository = _paymentRequestRepository;
            _validator = _paymentRequestValidation;   
        }

        public IQueryable<PaymentRequest> GetQueryable()
        {  
            return _repository.GetQueryable();
        }

        public PaymentRequest GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public PaymentRequest GetObjectByShipmentOrderId(int Id)
        {
            return _repository.GetObjectByShipmentOrderId(Id);
        }

        public void CreateContraOnEdit(PaymentRequest paymentRequest,IShipmentOrderService _shipmentOrderService,IPaymentRequestDetailService _paymentRequestDetailService)
        {
            PaymentRequest existPR = new PaymentRequest();
            existPR = _repository.GetObjectById(paymentRequest.Id);
            if (existPR != null)
            {
                var shipment = _shipmentOrderService.GetObjectById(existPR.ShipmentOrderId);
               // int jobCode = (from j in _shipmentOrderRepo.GetJobList() where j.Id == shipment.JobId select j.JobCode).FirstOrDefault();
                string shipmentNo = shipment.ShipmentOrderCode;

                // Update Existed PR as Deleted
                existPR.IsDeleted = true;
                existPR.DeletedAt = DateTime.Now;
                _repository.Update(existPR);

                int newPRContraNo = 0;
                string debetCredit = "";
                // As Debet
                if (existPR.DebetCredit == MasterConstant.DebetCredit.Debet)
                {
                    // Get Last PRNo as Contra
                    debetCredit = MasterConstant.DebetCredit.Credit;
                    newPRContraNo = _repository.GetLastPRNo(paymentRequest.OfficeId, MasterConstant.DebetCredit.Credit) + 1;
                }
                // As Credit
                else if (existPR.DebetCredit == MasterConstant.DebetCredit.Credit)
                {
                    // Get Last PRNo as Contra
                    debetCredit = MasterConstant.DebetCredit.Debet;
                    newPRContraNo = _repository.GetLastPRNo(paymentRequest.OfficeId, MasterConstant.DebetCredit.Credit) + 1;
                }

                // Get Last PRStatus
                int prStatusContra = _repository.GetLastPRStatus(paymentRequest.OfficeId, existPR.ShipmentOrderId);


                // Create Contra PR based on Updated PR
                PaymentRequest prNew = new PaymentRequest();
                prNew.PRNo = newPRContraNo;
                prNew.OfficeId = paymentRequest.OfficeId; 
                prNew.CreatedAt = DateTime.Now;
                prNew.ShipmentOrderId = existPR.ShipmentOrderId;
                prNew.CreatedById = paymentRequest.CreatedById;
                prNew.ContactId = existPR.ContactId;
                prNew.JenisPaymentRequest = existPR.JenisPaymentRequest;
                prNew.PaymentIDR = existPR.PaymentIDR;
                prNew.PaymentUSD = existPR.PaymentUSD;
                prNew.PersonalId = existPR.PersonalId;
                prNew.ContactTypeId = existPR.ContactTypeId;
                prNew.DebetCredit = debetCredit;
                prNew.PRStatus = prStatusContra;

                prNew.IsDeleted = true;
                prNew.DeletedAt = DateTime.Now;
                prNew.PRContraNo = existPR.PRNo;
                prNew.PRContraStatus = MasterConstant.Status.Cancel;
                prNew.Rate = existPR.Rate;
                prNew.ExRateDate = existPR.ExRateDate;
                prNew.PRNo = _repository.GetLastPRNo(paymentRequest.OfficeId, prNew.DebetCredit) + 1;
                prNew.PRStatus = _repository.GetLastPRStatus(paymentRequest.OfficeId, paymentRequest.ShipmentOrderId) + 1;
                prNew.Printing = 0;
                prNew.Reference = GeneratePaymentRequestReference(shipment.ShipmentOrderCode, prNew.PRNo.ToString());
                //
                prNew = _repository.CreateObject(prNew);

                // Create Contra PR Detail based on Updated PR
                List<PaymentRequestDetail> prdDetail = _paymentRequestDetailService.GetQueryable().Where(x => x.PaymentRequestId == existPR.Id).ToList();
                int sequence = 0;
                foreach (var item in prdDetail.ToList())
                {
                    PaymentRequestDetail prd = new PaymentRequestDetail();
                    prd.PaymentRequestId = prNew.Id;
                    prd.DebetCredit = debetCredit;
                    prd.CostId = item.CostId;
                    prd.Amount = item.Amount;
                    prd.AmountCrr = item.AmountCrr;
                    prd.CodingQuantity = item.CodingQuantity;
                    prd.OfficeId = item.OfficeId;
                    prd.CreatedById = paymentRequest.CreatedById;
                    //prd.DataFrom = false; 
                    prd.Description = item.Description;
                    prd.EPLDetailId = item.EPLDetailId;
                    prd.PerQty = item.PerQty;
                    prd.Quantity = item.Quantity;
                    prd.Type = item.Type;
                    prd.Paid = item.Paid;
                    if (item.Paid)
                        prd.PaidOn = DateTime.Now;

                    sequence++;
                    prd.Sequence = sequence;

                    _paymentRequestDetailService.CreateObject(prd,this);
                }
                // END Create Contra PR Detail based on Updated PR


                // ------------------------ Create New PR (Duplicate from existing) ------------------------

                // Create New PR based on Updated PR
                prNew = new PaymentRequest();
                prNew.PRNo = _repository.GetLastPRNo(existPR.OfficeId, existPR.DebetCredit) + 1;
                prNew.OfficeId = existPR.OfficeId; 
                prNew.CreatedAt = DateTime.Now;
                prNew.ShipmentOrderId = existPR.ShipmentOrderId;
                prNew.CreatedById = existPR.CreatedById; 
                prNew.Printing = 0; 
                prNew.ContactId = existPR.ContactId;
                prNew.JenisPaymentRequest = existPR.JenisPaymentRequest; 
                prNew.PaymentIDR = existPR.PaymentIDR; 
                prNew.PersonalId = existPR.PersonalId;
                prNew.ContactTypeId = existPR.ContactTypeId;
                prNew.DebetCredit = existPR.DebetCredit;
                prNew.PRStatus = _repository.GetLastPRStatus(paymentRequest.OfficeId,existPR.ShipmentOrderId);
                prNew.PRContraNo = existPR.PRNo; prNew.PRContraStatus = "Edit";
                prNew.Reference = GeneratePaymentRequestReference(shipmentNo, prNew.PRNo.ToString());
                prNew.Rate = existPR.Rate;
                prNew.ExRateDate = existPR.ExRateDate;
                prNew.PRStatus = _repository.GetLastPRStatus(existPR.OfficeId, existPR.ShipmentOrderId) + 1;
                //
                prNew = _repository.CreateObject(prNew);

                // Create New PR Detail based on Updated PR
                sequence = 0;
                foreach (var item in prdDetail.ToList())
                {
                    PaymentRequestDetail prd = new PaymentRequestDetail();
                    prd.PaymentRequestId = prNew.Id;
                    prd.DebetCredit = existPR.DebetCredit;
                    prd.CostId = item.CostId;
                    prd.Amount = item.Amount;
                    prd.AmountCrr = item.AmountCrr;
                    prd.CodingQuantity = item.CodingQuantity;
                    prd.OfficeId = item.OfficeId;
                    prd.CreatedById = paymentRequest.CreatedById;
                    //prd.DataFrom = false;
                    prd.Description = item.Description;
                    prd.EPLDetailId = item.EPLDetailId;
                    prd.PerQty = item.PerQty;
                    prd.Quantity = item.Quantity;
                    prd.Type = item.Type;
                    prd.Paid = item.Paid;
                    if (item.Paid)
                        prd.PaidOn = DateTime.Now;

                    sequence++;
                    prd.Sequence = sequence;

                    _paymentRequestDetailService.CreateObject(prd, this);
                }
                // END Create New PR Detail based on Updated PR
            }
            else
            {
            }
        }

        public PaymentRequest Paid(PaymentRequest paymentRequest)
        {
            paymentRequest.Paid = true;
            paymentRequest.DatePaid = DateTime.Today;
            paymentRequest = _repository.UpdateObject(paymentRequest);
            return paymentRequest;
        }

        public PaymentRequest Unpaid(PaymentRequest paymentRequest)
        {
            paymentRequest.Paid = false;
            paymentRequest.DatePaid = null;
            paymentRequest = _repository.UpdateObject(paymentRequest);
            return paymentRequest;
        }


        public PaymentRequest CreateObject(PaymentRequest paymentRequest,IShipmentOrderService _shipmentOrderService)
        {
            paymentRequest.Errors = new Dictionary<String, String>();
            if (isValid(_validator.VCreateObject(paymentRequest,_shipmentOrderService)))
            {
                PaymentRequest newPR = new PaymentRequest();
                newPR.OfficeId = paymentRequest.OfficeId;
                newPR.CreatedById = paymentRequest.CreatedById;
                newPR.CreatedAt = DateTime.Now;
                newPR.ContactId = paymentRequest.ContactId;
                newPR.ContactTypeId = paymentRequest.ContactTypeId;
                newPR.DebetCredit = MasterConstant.DebetCredit.Credit;
                newPR.JenisPaymentRequest = paymentRequest.JenisPaymentRequest;
                newPR.PaymentIDR = paymentRequest.PaymentIDR;
                newPR.PaymentUSD = paymentRequest.PaymentUSD;
                newPR.PersonalId = paymentRequest.PersonalId;
                newPR.ShipmentOrderId = paymentRequest.ShipmentOrderId;
                newPR.Rate = paymentRequest.Rate;
                newPR.PRNo = _repository.GetLastPRNo(paymentRequest.OfficeId,newPR.DebetCredit) + 1;
                newPR.PRStatus = _repository.GetLastPRStatus(paymentRequest.OfficeId,paymentRequest.ShipmentOrderId) + 1;
                newPR.PaymentRequestDate = DateTime.Today;
                newPR.Printing = 0;
                newPR.CurrencyId = paymentRequest.CurrencyId;
                ShipmentOrder shipment =_shipmentOrderService.GetObjectById(newPR.ShipmentOrderId);
                if (shipment != null)
                 {
                     newPR.Reference = GeneratePaymentRequestReference(shipment.ShipmentOrderCode,newPR.PRNo.ToString());
                 }
                newPR = _repository.CreateObject(newPR);
                paymentRequest.Id = newPR.Id;
            }
            return paymentRequest;
        }

        public static string Replicate(string codeVal, int length)
        {
            if (String.IsNullOrEmpty(codeVal))
            {
                return codeVal;
            }

            string result = "";
            for (int i = codeVal.Length; i < length; i++)
            {
                result += "0";
            }
            result += codeVal;

            return result;
        }

        private string GeneratePaymentRequestReference(string shipmentNo, string prNo)
        {
            string prReference = "";
            prReference = "PR/" + shipmentNo.Trim().Substring(shipmentNo.Trim().IndexOf("."), 12) + "/" + Replicate(prNo.ToString(), 6) + "/" + DateTime.Now.Year.ToString();
            return prReference;
        }

        private string GeneratePaymentRequestNo(int prNo)
        {
            string PR = "";
            PR = Replicate(prNo.ToString(), 6);
            return PR;
        }

        public PaymentRequest UpdateObject(PaymentRequest paymentRequest,IShipmentOrderService _shipmentOrderService)
        {
            if (isValid(_validator.VUpdateObject(paymentRequest, this,_shipmentOrderService)))
            {
                paymentRequest = _repository.UpdateObject(paymentRequest);
            }
            return paymentRequest;
        }

        public void UpdateObjectAfterPrint(int officeId,int Id,string alljob,ExchangeRateService _exchangeRateService)
        {
            if (alljob == "y")
            {
                var shipmentid = _repository.GetObjectById(Id).ShipmentOrderId;
                var job = _repository.GetQueryable().Where(x => x.ShipmentOrderId == shipmentid).ToList();
                foreach (var item in job)
                {
                    PaymentRequest updatePR = _repository.GetObjectById(item.Id);

                    if (updatePR != null)
                    {
                        if (updatePR.Printing > 0)
                        {
                            updatePR.Printing = updatePR.Printing + 1;
                        }
                        else
                        {
                            updatePR.Printing = 1;
                            if (updatePR.PrintedAt == null)
                            {
                                updatePR.PrintedAt = DateTime.Now;
                            }
                            ExchangeRate rateInfo = _exchangeRateService.GetQueryable()
                                .Where(x => x.ExRateDate <= DateTime.Today && x.OfficeId == officeId)
                                .OrderByDescending(x => x.ExRateDate).FirstOrDefault();
                            if (rateInfo != null) 
                            {
                                updatePR.ExRateId = rateInfo.Id;
                                updatePR.ExRateDate = rateInfo.ExRateDate;
                                updatePR.Rate = rateInfo.ExRate1;
                            }

                            // CONTRA PR
                            if (!String.IsNullOrEmpty(updatePR.PRContraStatus)
                                && (updatePR.PRContraStatus.Length > 1 && updatePR.PRContraStatus.Substring(0, 1).ToUpper() == MasterConstant.DebetCredit.Credit))
                            {
                                string contraDebetCredit = updatePR.DebetCredit == MasterConstant.DebetCredit.Debet
                                             ? MasterConstant.DebetCredit.Credit : MasterConstant.DebetCredit.Debet;

                                PaymentRequest oriPR = _repository.Find(i => i.DebetCredit == contraDebetCredit && i.OfficeId == officeId
                                                                            && i.PRNo == updatePR.PRContraNo);
                                if (oriPR != null)
                                {
                                    ExchangeRate oriRateInfo = _exchangeRateService.GetQueryable().Where(x => x.ExRateDate <= oriPR.ExRateDate && x.OfficeId == officeId)
                                                               .OrderByDescending(x => x.ExRateDate).FirstOrDefault();
                                    updatePR.ExRateId = oriRateInfo.Id;
                                    updatePR.Rate = oriPR.Rate;
                                    updatePR.ExRateDate = oriPR.ExRateDate;
                                }
                            }
                            // END CONTRA PR

                        }
                        _repository.Update(updatePR);
                    }
                }
            }
            else if (alljob == "n")
            {
                PaymentRequest updatePR = _repository.GetObjectById(Id);
                if (updatePR != null)
                {
                    if (updatePR.Printing > 0)
                    {
                        updatePR.Printing = updatePR.Printing + 1;
                    }
                    else
                    {
                        ExchangeRate rateInfo = _exchangeRateService.GetQueryable().Where(x => x.ExRateDate <= DateTime.Today && x.OfficeId == officeId)
                                                .OrderByDescending(x => x.ExRateDate).FirstOrDefault();
                        if (rateInfo != null)
                        {
                            updatePR.ExRateId = rateInfo.Id;
                            updatePR.ExRateDate = rateInfo.ExRateDate;
                            updatePR.Rate = rateInfo.ExRate1;
                        }
                        updatePR.Printing = 1;
                        if (updatePR.PrintedAt == null)
                        {
                            updatePR.PrintedAt = DateTime.Now;
                        }

                        // CONTRA PR
                        if (!String.IsNullOrEmpty(updatePR.PRContraStatus)
                               && (updatePR.PRContraStatus.Length > 1 && updatePR.PRContraStatus.Substring(0, 1).ToUpper() == MasterConstant.DebetCredit.Credit))
                        {
                            string contraDebetCredit = updatePR.DebetCredit == MasterConstant.DebetCredit.Debet
                                           ? MasterConstant.DebetCredit.Credit : MasterConstant.DebetCredit.Debet;
                            PaymentRequest oriPR = _repository.Find(i => i.DebetCredit == contraDebetCredit && i.OfficeId == officeId
                                                                          && i.PRNo == updatePR.PRContraNo);
                            if (oriPR != null)
                            {
                                ExchangeRate oriRateInfo = _exchangeRateService.GetQueryable().Where(x => x.ExRateDate <= oriPR.ExRateDate && x.OfficeId == officeId)
                                                           .OrderByDescending(x => x.ExRateDate).FirstOrDefault();
                                updatePR.ExRateId = oriRateInfo.Id;
                                updatePR.Rate = oriPR.Rate;
                                updatePR.ExRateDate = oriPR.ExRateDate;
                            }
                        }
                        // END CONTRA PR
                    }
                    _repository.Update(updatePR);
                }
            }
        }

        public PaymentRequest ConfirmObject(PaymentRequest paymentRequest, DateTime confirmationDate, IPaymentRequestDetailService _paymentRequestDetailService, IPayableService _payableService)
        {
            if (isValid(_validator.VConfirmObject(paymentRequest,_paymentRequestDetailService)))
            {
                IList<PaymentRequestDetail> paymentRequestDetails = _paymentRequestDetailService.GetQueryable().Where(x => x.PaymentRequestId == paymentRequest.Id && x.IsDeleted == false).ToList();
                foreach (var paymentRequestDetail in paymentRequestDetails)
                {
                    paymentRequestDetail.Errors = new Dictionary<string, string>();
                    paymentRequestDetail.ConfirmationDate = confirmationDate;
                    _paymentRequestDetailService.ConfirmObject(paymentRequestDetail);
                    _payableService.CreateObject(paymentRequest.OfficeId,paymentRequest.ContactId.Value, MasterConstant.SourceDocument.PaymentRequest, paymentRequest.Id,paymentRequestDetail.Id, 
                        paymentRequestDetail.AmountCrr ?? 0, paymentRequestDetail.Amount ?? 0, paymentRequest.Rate ?? 0, paymentRequest.PaymentRequestDate);
                }
                paymentRequest.ConfirmationDate = confirmationDate;
                paymentRequest = _repository.ConfirmObject(paymentRequest);
      
            }
                 return paymentRequest;
        }

        public PaymentRequest UnconfirmObject(PaymentRequest paymentRequest, IPaymentRequestDetailService _paymentRequestDetailService, IPayableService _payableService,IPaymentVoucherDetailService _paymentVoucherDetailService)
        {
            if (isValid(_validator.VUnconfirmObject(paymentRequest,_payableService,_paymentVoucherDetailService)))
            {
                IList<PaymentRequestDetail> paymentRequestDetails = _paymentRequestDetailService.GetQueryable().Where(x => x.PaymentRequestId == paymentRequest.Id && x.IsDeleted == false).ToList();
                foreach (var paymentRequestDetail in paymentRequestDetails)
                {
                    paymentRequestDetail.Errors = new Dictionary<string, string>();
                    _paymentRequestDetailService.UnconfirmObject(paymentRequestDetail);
                    Payable payable = _payableService.GetObjectBySource(MasterConstant.SourceDocument.PaymentRequest, paymentRequest.Id,paymentRequestDetail.Id);
                    _payableService.SoftDeleteObject(payable);
                }
                paymentRequest = _repository.UnconfirmObject(paymentRequest);
            }
            return paymentRequest;
        }

        public PaymentRequest SoftDeleteObject(PaymentRequest paymentRequest)
        {
            if (isValid(_validator.VSoftDeleteObject(paymentRequest,this)))
            {
                paymentRequest = _repository.SoftDeleteObject(paymentRequest);
            }
            return paymentRequest;
        }

        public PaymentRequest CalculateTotalPaymentRequest(PaymentRequest paymentRequest, IPaymentRequestDetailService _paymentRequestDetailService)
        {
                decimal totalIDR = 0;
                decimal totalUSD = 0;
                var prDetailList = _paymentRequestDetailService.GetQueryable().Where(x => x.PaymentRequestId == paymentRequest.Id 
                    && x.IsDeleted == false);

                foreach (var item in prDetailList)
                {
                    if (item.AmountCrr == MasterConstant.Currency.IDR)
                    {
                        totalIDR += item.Amount.HasValue ? item.Amount.Value : 0;
                    }
                    else
                    {
                        totalUSD += item.Amount.HasValue ? item.Amount.Value : 0;
                    }
                  
                }
                paymentRequest.PaymentIDR = totalIDR;
                paymentRequest.PaymentUSD = totalUSD;
                _repository.UpdateObject(paymentRequest);
                return paymentRequest;
        }



        public bool isValid(PaymentRequest obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }
    }
}
