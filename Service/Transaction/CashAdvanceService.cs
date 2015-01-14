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
    public class CashAdvanceService : ICashAdvanceService 
    {  
        private ICashAdvanceRepository _repository;
        private ICashAdvanceValidation _validator;

        public CashAdvanceService(ICashAdvanceRepository _cashAdvanceRepository, ICashAdvanceValidation _cashAdvanceValidation)
        {
            _repository = _cashAdvanceRepository;
            _validator = _cashAdvanceValidation;
        }

        public IQueryable<CashAdvance> GetQueryable()
        {  
            return _repository.GetQueryable();
        }

        public CashAdvance GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
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

        private string GenerateCashBondReference(CashAdvance cashAdvance)
        {
            string reference = "";
            reference = "CAD/" + Replicate(cashAdvance.CashAdvanceNo.ToString(), 6) + "/" + cashAdvance.CreatedAt.ToString("MM") + "-" + cashAdvance.CreatedAt.ToString("yyyy");
            return reference;
        }

        public CashAdvance CalculateTotalAmount(CashAdvance cashAdvance, ICashAdvanceDetailService _cashAdvanceDetailService)
        {
            IList<CashAdvanceDetail> cashAdvanceDetails = _cashAdvanceDetailService.GetQueryable().Where(x=>x.CashAdvanceId == cashAdvance.Id && x.IsDeleted ==false).ToList();
            decimal totalIDR = 0;
            decimal totalUSD = 0;

            foreach (CashAdvanceDetail detail in cashAdvanceDetails)
            {
                totalIDR += detail.AmountIDR;
                totalUSD += detail.AmountUSD;
            }
            cashAdvance.CashAdvanceIDR = totalIDR;
            cashAdvance.CashAdvanceUSD = totalUSD;
            cashAdvance = _repository.UpdateObject(cashAdvance);
            return cashAdvance;
        }

        public CashAdvance CreateObject(CashAdvance cashAdvance,IShipmentOrderService _shipmentOrderService,IContactService _contactService)
        {
            cashAdvance.Errors = new Dictionary<String, String>();
            if (isValid(_validator.VCreateObject(cashAdvance,this)))
            {
                CashAdvance newCashAdvance  = new CashAdvance();
                newCashAdvance.IsDeleted = false;
                newCashAdvance.OfficeId = cashAdvance.OfficeId;
                newCashAdvance.CreatedById = cashAdvance.CreatedById;
                newCashAdvance.CreatedAt = DateTime.Today;
                newCashAdvance.Rate = cashAdvance.Rate;
                newCashAdvance.EmployeeId = cashAdvance.EmployeeId;
                newCashAdvance.CashAdvanceTo = cashAdvance.CashAdvanceTo;
                newCashAdvance.CashAdvanceNo = _repository.GetCashAdvanceNo(cashAdvance.OfficeId) + 1;
                newCashAdvance.Reference = this.GenerateCashBondReference(newCashAdvance);
                newCashAdvance.Errors = new Dictionary<String, String>();
                cashAdvance = _repository.CreateObject(newCashAdvance);
            }
            return cashAdvance;
        }

       

        public CashAdvance UpdateObject(CashAdvance cashAdvance)
        {
            if (isValid(_validator.VUpdateObject(cashAdvance, this)))
            {
                cashAdvance = _repository.UpdateObject(cashAdvance);
            }
            return cashAdvance;
        }
          
        public CashAdvance SoftDeleteObject(CashAdvance cashAdvance,ICashAdvanceDetailService _cashAdvanceDetailService)
        {
            cashAdvance = _repository.SoftDeleteObject(cashAdvance);
            IList<CashAdvanceDetail> estimateProfitLossDetail = _cashAdvanceDetailService.GetQueryable().
                                                                       Where(x => x.CashAdvanceId == cashAdvance.Id).ToList();
            foreach(var item in estimateProfitLossDetail)
            {
                _cashAdvanceDetailService.SoftDeleteObject(item,this);
            }
            return cashAdvance;
        }


        public bool isValid(CashAdvance obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public CashAdvance ConfirmObject(CashAdvance cashAdvance, DateTime ConfirmationDate,ICashAdvanceDetailService _cashAdvanceDetailService)
        {
            cashAdvance.ConfirmationDate = ConfirmationDate;
             if (isValid(_validator.VConfirmObject(cashAdvance,this)))
            {
                IList<CashAdvanceDetail> details = _cashAdvanceDetailService.GetQueryable().Where(x=>x.CashAdvanceId == cashAdvance.Id && x.IsDeleted ==false).ToList();
                foreach (var detail in details)
                {
                    detail.Errors = new Dictionary<string, string>();
                    _cashAdvanceDetailService.ConfirmObject(detail, ConfirmationDate);
                }

                _repository.ConfirmObject(cashAdvance);
            }
            return cashAdvance;
        }

        public CashAdvance UnconfirmObject(CashAdvance cashAdvance, ICashAdvanceDetailService _cashAdvanceDetailService)
        {
            if (isValid(_validator.VUnconfirmObject(cashAdvance, this)))
            {
                IList<CashAdvanceDetail> details = _cashAdvanceDetailService.GetQueryable().Where(x => x.CashAdvanceId == cashAdvance.Id && x.IsDeleted == false).ToList();
                foreach (var detail in details)
                {
                    detail.Errors = new Dictionary<string, string>();
                    _cashAdvanceDetailService.UnconfirmObject(detail);
                }

                _repository.UnconfirmObject(cashAdvance);
            }
            return cashAdvance;
        }

    }
}
