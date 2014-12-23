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
    public class CashAdvanceDetailValidation : ICashAdvanceDetailValidation
    {  
        
        public CashAdvanceDetail VCreateObject(CashAdvanceDetail cashAdvanceDetail, ICashAdvanceDetailService _cashAdvanceDetailService)
        {
            return cashAdvanceDetail;
        }

        public CashAdvanceDetail VUpdateObject(CashAdvanceDetail cashAdvanceDetail, ICashAdvanceDetailService _cashAdvanceDetailService)
        { 
            return cashAdvanceDetail;
        }

        public bool isValid(CashAdvanceDetail obj)
        { 
            bool isValid = !obj.Errors.Any();
            return isValid;
        }
    }
}
