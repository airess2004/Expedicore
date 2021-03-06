﻿using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validation.Validation
{ 
    public class EstimateProfitLossValidation : IEstimateProfitLossValidation
    {  
        
        public EstimateProfitLoss VCreateObject(EstimateProfitLoss estimateprofitloss, IEstimateProfitLossService _estimateprofitlossService)
        {
            return estimateprofitloss;
        }

        public EstimateProfitLoss VUpdateObject(EstimateProfitLoss estimateprofitloss, IEstimateProfitLossService _estimateprofitlossService)
        { 
            return estimateprofitloss;
        }

        public bool isValid(EstimateProfitLoss obj)
        { 
            bool isValid = !obj.Errors.Any();
            return isValid;
        }
    }
}
