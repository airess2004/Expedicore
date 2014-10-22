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
    public class EstimateProfitLossDetailValidation : IEstimateProfitLossDetailValidation
    {  
        
        public EstimateProfitLossDetail VCreateObject(EstimateProfitLossDetail estimateprofitlossdetail, IEstimateProfitLossDetailService _estimateprofitlossdetailService)
        {
            return estimateprofitlossdetail;
        }

        public EstimateProfitLossDetail VUpdateObject(EstimateProfitLossDetail estimateprofitlossdetail, IEstimateProfitLossDetailService _estimateprofitlossdetailService)
        { 
            return estimateprofitlossdetail;
        }

        public bool isValid(EstimateProfitLossDetail obj)
        { 
            bool isValid = !obj.Errors.Any();
            return isValid;
        }
    }
}
