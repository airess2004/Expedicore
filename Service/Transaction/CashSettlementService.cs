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
    public class CashSettlementService : ICashSettlementService 
    {  
        private ICashSettlementRepository _repository;
        private ICashSettlementValidation _validator;

        public CashSettlementService(ICashSettlementRepository _cashSettlementRepository, ICashSettlementValidation _cashSettlementValidation)
        {
            _repository = _cashSettlementRepository;
            _validator = _cashSettlementValidation;
        }

        public IQueryable<CashSettlement> GetQueryable()
        {  
            return _repository.GetQueryable();
        }

        public CashSettlement GetObjectById(int Id)
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

        private string GenerateCashBondReference(CashSettlement cashSettlement)
        {
            string reference = "";
            reference = "STE/" + cashSettlement.SettlementNo + "/" + cashSettlement.CreatedAt.ToString("MM") + "-" + cashSettlement.CreatedAt.ToString("yyyy");
            return reference;
        }

        public CashSettlement CreateObject(CashSettlement cashSettlement)
        {
            cashSettlement.Errors = new Dictionary<String, String>();
            if (isValid(_validator.VCreateObject(cashSettlement,this)))
            {
                CashSettlement newCashSettlement  = new CashSettlement();
                newCashSettlement.IsDeleted = false;
                newCashSettlement.OfficeId = cashSettlement.OfficeId;
                newCashSettlement.CreatedById = cashSettlement.CreatedById;
                newCashSettlement.CreatedAt = DateTime.Today;
                newCashSettlement.Rate = cashSettlement.Rate;
                newCashSettlement.EmployeeId = cashSettlement.EmployeeId;
                newCashSettlement.SettlementUSD = cashSettlement.SettlementUSD;
                newCashSettlement.SettlementIDR = cashSettlement.SettlementIDR;
                newCashSettlement.Rate = cashSettlement.Rate;
                newCashSettlement.EmployeeId = cashSettlement.EmployeeId;
                newCashSettlement.CashAdvanceId = cashSettlement.CashAdvanceId;
                newCashSettlement.SettlementNo = _repository.GetCashSettlementNo(cashSettlement.OfficeId) + 1;
                newCashSettlement.Reference = this.GenerateCashBondReference(newCashSettlement);
                newCashSettlement = _repository.CreateObject(newCashSettlement);
            }
            return cashSettlement;
        }


        public CashSettlement UpdateObject(CashSettlement cashSettlement)
        {
            if (isValid(_validator.VUpdateObject(cashSettlement, this)))
            {
                cashSettlement = _repository.UpdateObject(cashSettlement);
            }
            return cashSettlement;
        }
          
        public CashSettlement SoftDeleteObject(CashSettlement cashSettlement)
        {
            cashSettlement = _repository.SoftDeleteObject(cashSettlement);
         
            return cashSettlement;
        }


        public bool isValid(CashSettlement obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }
    }
}
