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
    public class CashSettlementValidation : ICashSettlementValidation
    {  
        
        public CashSettlement VCreateObject(CashSettlement cashSettlement, ICashSettlementService _cashSettlementService)
        {
            return cashSettlement;
        }

        public CashSettlement VUpdateObject(CashSettlement cashSettlement, ICashSettlementService _cashSettlementService)
        {
            return cashSettlement;
        }

        public CashSettlement VConfirmObject(CashSettlement cashSettlement, ICashSettlementService _cashSettlementService)
        {
            return cashSettlement; 
        }


        public CashSettlement VUnconfirmObject(CashSettlement cashSettlement, ICashSettlementService _cashSettlementService)
        {
            return cashSettlement;
        }

        public bool isValid(CashSettlement obj)
        { 
            bool isValid = !obj.Errors.Any();
            return isValid;
        }
    }
}
