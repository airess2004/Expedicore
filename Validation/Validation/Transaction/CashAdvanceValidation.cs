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
    public class CashAdvanceValidation : ICashAdvanceValidation
    {  
        
        public CashAdvance VCreateObject(CashAdvance cashAdvance, ICashAdvanceService _cashAdvanceService)
        {
            return cashAdvance;
        }

        public CashAdvance VUpdateObject(CashAdvance cashAdvance, ICashAdvanceService _cashAdvanceService)
        {
            return cashAdvance;
        }

        public CashAdvance VConfirmObject(CashAdvance cashAdvance, ICashAdvanceService _cashAdvanceService)
        {
            return cashAdvance; 
        }


        public CashAdvance VUnconfirmObject(CashAdvance cashAdvance, ICashAdvanceService _cashAdvanceService)
        {
            return cashAdvance;
        }

        public bool isValid(CashAdvance obj)
        { 
            bool isValid = !obj.Errors.Any();
            return isValid;
        }
    }
}
