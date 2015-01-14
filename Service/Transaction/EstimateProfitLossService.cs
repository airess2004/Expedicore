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
    public class EstimateProfitLossService : IEstimateProfitLossService 
    {  
        private IEstimateProfitLossRepository _repository;
        private IEstimateProfitLossValidation _validator;

        public EstimateProfitLossService(IEstimateProfitLossRepository _estimateprofitlossRepository, IEstimateProfitLossValidation _estimateprofitlossValidation)
        {
            _repository = _estimateprofitlossRepository;
            _validator = _estimateprofitlossValidation;
        }

        public IQueryable<EstimateProfitLoss> GetQueryable()
        {  
            return _repository.GetQueryable();
        }

        public EstimateProfitLoss GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public EstimateProfitLoss GetObjectByShipmentOrderId(int Id)
        {
            return _repository.GetObjectByShipmentOrderId(Id);
        }

        public EstimateProfitLoss CreateUpdateObject(EstimateProfitLoss estimateprofitloss,IShipmentOrderService _shipmentOrderService)
        {
            EstimateProfitLoss newestimateprofitloss = this.GetObjectByShipmentOrderId(estimateprofitloss.ShipmentOrderId);
            if (newestimateprofitloss == null)
            {
               // estimateprofitloss = this.CreateObject(estimateprofitloss,_shipmentOrderService,_exchangeRateService);
            }
            else
            {
                estimateprofitloss = this.UpdateObject(estimateprofitloss,_shipmentOrderService);
            }
            return estimateprofitloss;
        }

        public EstimateProfitLoss CreateObject(EstimateProfitLoss estimateprofitloss,IShipmentOrderService _shipmentOrderService)
        {
            estimateprofitloss.Errors = new Dictionary<String, String>();
            if (isValid(_validator.VCreateObject(estimateprofitloss,this,_shipmentOrderService)))
            {
                EstimateProfitLoss newEPL = new EstimateProfitLoss();
                newEPL.CloseEPL = false;
                newEPL.IsDeleted = false;
                newEPL.OfficeId = estimateprofitloss.OfficeId;
                newEPL.CreatedById = estimateprofitloss.CreatedById;
                newEPL.CreatedAt = DateTime.Today;
                newEPL.ShipmentOrderId = estimateprofitloss.ShipmentOrderId;
                //newEPL.ExchangeRateId = _exchangeRateService.GetLatestRate(DateTime.Today).Id;
                newEPL.Printing = 0;
                newEPL.Errors = estimateprofitloss.Errors;
                estimateprofitloss = _repository.CreateObject(newEPL);


                ShipmentOrder shipmentOrder = _shipmentOrderService.GetObjectById(estimateprofitloss.ShipmentOrderId);
                if (shipmentOrder != null)
                {
                    if (shipmentOrder.SubJobNumber == 0 && shipmentOrder.TotalSub > 0)
                    {
                        IList<ShipmentOrder> shipmentSubJob = _shipmentOrderService.GetQueryable().Where(x => x.JobNumber == shipmentOrder.JobNumber
                                                               && x.SubJobNumber > 0 && x.JobId == shipmentOrder.JobId).ToList();
                        foreach (var item in shipmentSubJob)
                        {
                            EstimateProfitLoss newSubJob = new EstimateProfitLoss();
                            newSubJob.CloseEPL = false;
                            newSubJob.IsDeleted = false;
                            newSubJob.OfficeId = estimateprofitloss.OfficeId;
                            newSubJob.CreatedById = estimateprofitloss.CreatedById;
                            newSubJob.CreatedAt = DateTime.Today;
                            newSubJob.ShipmentOrderId = item.Id;
                            newSubJob.Printing = 0;
                            _repository.CreateObject(newSubJob);
                        }
                    }
                }
            }
            return estimateprofitloss;
        }

        public EstimateProfitLoss CalculateTotalUSDIDR(int eplId,IEstimateProfitLossDetailService _estimateProfitLossDetailService)
        {
            IList<EstimateProfitLossDetail> eplD = _estimateProfitLossDetailService.GetQueryable().Where(x => x.EstimateProfitLossId == eplId && x.IsDeleted == false).ToList();
            decimal CostIDR = 0;
            decimal CostUSD = 0;
            decimal IncomeIDR = 0;
            decimal IncomeUSD = 0;
            foreach (var item in eplD)
            {
                if (item.IsIncome == true)
                {
                    if (item.AmountCrr == MasterConstant.Currency.IDR)
                    {
                        IncomeIDR += (item.AmountIDR.HasValue ? item.AmountIDR.Value : 0);
                    }
                    else
                    {
                        IncomeUSD += (item.AmountUSD.HasValue ? item.AmountUSD.Value : 0);

                    }
                }
                else
                {
                    if (item.AmountCrr == MasterConstant.Currency.IDR)
                    {
                        CostIDR += (item.AmountIDR.HasValue ? item.AmountIDR.Value : 0);
                    }
                    else
                    {
                        CostUSD += (item.AmountUSD.HasValue ? item.AmountUSD.Value : 0);
                    }
                }
            }
            EstimateProfitLoss epl = GetObjectById(eplId);
            epl.TotalCostIDR = CostIDR;
            epl.TotalCostUSD = CostUSD;
            epl.TotalIncomeIDR = IncomeIDR;
            epl.TotalIncomeUSD = IncomeUSD;
            epl = _repository.UpdateObject(epl);
            return epl;
        }

        public EstimateProfitLoss UpdateObject(EstimateProfitLoss estimateprofitloss,IShipmentOrderService _shipmentOrderService)
        {
            if (isValid(_validator.VUpdateObject(estimateprofitloss, this, _shipmentOrderService)))
            {
                estimateprofitloss = _repository.UpdateObject(estimateprofitloss);
            }
            return estimateprofitloss;
        }
          
        public EstimateProfitLoss SoftDeleteObject(EstimateProfitLoss estimateprofitloss,IEstimateProfitLossDetailService _estimateProfitLossDetailService)
        {
            if (isValid(_validator.VSoftDeleteObject(estimateprofitloss, this)))
            {
                estimateprofitloss = _repository.SoftDeleteObject(estimateprofitloss);
                IList<EstimateProfitLossDetail> estimateProfitLossDetail = _estimateProfitLossDetailService.GetQueryable().
                                                                           Where(x => x.EstimateProfitLossId == estimateprofitloss.Id && x.IsDeleted == false).ToList();
                foreach (var item in estimateProfitLossDetail)
                {
                    _estimateProfitLossDetailService.SoftDeleteObject(item);
                }
            }
            return estimateprofitloss;
        }

        public EstimateProfitLoss ConfirmObject(EstimateProfitLoss estimateProfitLoss, DateTime confirmationDate, IEstimateProfitLossDetailService _estimateProfitLossDetailService)
        {
            if (isValid(_validator.VConfirmObject(estimateProfitLoss,this)))
            { 
                IList<EstimateProfitLossDetail> estimateProfitLossDetails = _estimateProfitLossDetailService.GetQueryable().Where(x => x.EstimateProfitLossId == estimateProfitLoss.Id && x.IsDeleted == false).ToList();
                foreach (var estimateProfitLossDetail in estimateProfitLossDetails)
                {
                    estimateProfitLossDetail.Errors = new Dictionary<string, string>();
                    estimateProfitLossDetail.ConfirmationDate = confirmationDate;
                    _estimateProfitLossDetailService.ConfirmObject(estimateProfitLossDetail);
                }
                estimateProfitLoss.ConfirmationDate = confirmationDate;
                estimateProfitLoss = _repository.ConfirmObject(estimateProfitLoss);
            }
            return estimateProfitLoss;
        }

        public EstimateProfitLoss UnconfirmObject(EstimateProfitLoss estimateProfitLoss, DateTime confirmationDate, 
            IEstimateProfitLossDetailService _estimateProfitLossDetailService,IInvoiceDetailService _invoiceDetailService, 
            IPaymentRequestDetailService _paymentRequestDetailService)
        {
            if (isValid(_validator.VUnConfirmObject(estimateProfitLoss, this,_estimateProfitLossDetailService,_invoiceDetailService,_paymentRequestDetailService)))
            {
                IList<EstimateProfitLossDetail> estimateProfitLossDetails = _estimateProfitLossDetailService.GetQueryable().Where(x => x.EstimateProfitLossId == estimateProfitLoss.Id && x.IsDeleted == false).ToList();
                foreach (var estimateProfitLossDetail in estimateProfitLossDetails)
                {
                    estimateProfitLossDetail.Errors = new Dictionary<string, string>();
                    _estimateProfitLossDetailService.UnconfirmObject(estimateProfitLossDetail);
                }
                estimateProfitLoss = _repository.UnconfirmObject(estimateProfitLoss);
            }
            return estimateProfitLoss;
        }

        public bool isValid(EstimateProfitLoss obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }
    }
}
