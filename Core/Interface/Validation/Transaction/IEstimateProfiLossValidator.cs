using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Validation
{
    public interface IEstimateProfitLossValidation
    {
        EstimateProfitLoss VCreateObject(EstimateProfitLoss estimateprofitloss, IEstimateProfitLossService _estimateprofitlossService);
        EstimateProfitLoss VUpdateObject(EstimateProfitLoss estimateprofitloss, IEstimateProfitLossService _estimateprofitlossService);
    }
}
