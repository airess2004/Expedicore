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
    public class CashAdvanceDetailService : ICashAdvanceDetailService 
    {  
        private ICashAdvanceDetailRepository _repository;
        private ICashAdvanceDetailValidation _validator;

        public CashAdvanceDetailService(ICashAdvanceDetailRepository _cashAdvanceDetailRepository, ICashAdvanceDetailValidation _cashAdvanceDetailValidation)
        {
            _repository = _cashAdvanceDetailRepository;
            _validator = _cashAdvanceDetailValidation;
        }

        public IQueryable<CashAdvanceDetail> GetQueryable()
        {  
            return _repository.GetQueryable();
        }

        public CashAdvanceDetail GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public CashAdvanceDetail GetObjectByCashAdvanceId(int Id)
        {
            return _repository.GetObjectByCashAdvanceId(Id);
        }

        public CashAdvanceDetail CreateObject(CashAdvanceDetail cashAdvanceDetail, IShipmentOrderService _shipmentOrderService
            ,ICashAdvanceService _cashAdvanceService)
        { 
            cashAdvanceDetail.Errors = new Dictionary<String, String>();
            if (isValid(_validator.VCreateObject(cashAdvanceDetail,this)))
            {
                CashAdvanceDetail newCashBondDetail = new CashAdvanceDetail();
                newCashBondDetail.CashAdvanceId = cashAdvanceDetail.CashAdvanceId;
                newCashBondDetail.Description = !String.IsNullOrEmpty(cashAdvanceDetail.Description) ? cashAdvanceDetail.Description.ToUpper() : "";
                newCashBondDetail.AmountUSD = cashAdvanceDetail.AmountUSD;
                newCashBondDetail.AmountIDR = cashAdvanceDetail.AmountIDR;
                newCashBondDetail.PaymentRequestDetailId = cashAdvanceDetail.PaymentRequestDetailId;
                newCashBondDetail.ShipmentOrderId = cashAdvanceDetail.ShipmentOrderId;
                if (newCashBondDetail.ShipmentOrderId != null)
                {
                    var so = _shipmentOrderService.GetObjectById(cashAdvanceDetail.ShipmentOrderId.Value);
                    if (so != null)
                    {
                        newCashBondDetail.ShipmentNo = so.ShipmentOrderCode;
                    }
                }

                cashAdvanceDetail = _repository.CreateObject(newCashBondDetail);
                ReCalculateTotal(cashAdvanceDetail.CashAdvanceId, _cashAdvanceService);
            }
            return cashAdvanceDetail;
        }

        private void ReCalculateTotal(int cashBondId , ICashAdvanceService _cashAdvanceService)
        {
            var cashBond = _cashAdvanceService.GetObjectById(cashBondId);
            if (cashBond != null)
            {
                decimal totalUSD = 0;
                decimal totalIDR = 0;
                var cashBondDetails = _repository.GetQueryable().Where(x => x.CashAdvanceId == cashBondId && x.IsDeleted == false).ToList();
                foreach (var item in cashBondDetails)
                {
                    totalUSD += item.AmountUSD ?? 0;
                    totalIDR += item.AmountIDR ?? 0;
                }

                cashBond.CashAdvanceUSD = totalUSD;
                cashBond.CashAdvanceIDR = totalIDR;

                _cashAdvanceService.UpdateObject(cashBond);
            }
        }
     
        public CashAdvanceDetail UpdateObject(CashAdvanceDetail cashAdvanceDetail,ICashAdvanceService _cashAdvanceService)
        {
            if (isValid(_validator.VUpdateObject(cashAdvanceDetail, this)))
            {
                cashAdvanceDetail = _repository.UpdateObject(cashAdvanceDetail);
                ReCalculateTotal(cashAdvanceDetail.CashAdvanceId, _cashAdvanceService);
            }
            return cashAdvanceDetail;
        }
         
        public CashAdvanceDetail SoftDeleteObject(CashAdvanceDetail cashAdvanceDetail)
        {
            cashAdvanceDetail = _repository.SoftDeleteObject(cashAdvanceDetail);
            return cashAdvanceDetail;
        }


        public bool isValid(CashAdvanceDetail obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }
    }
}
