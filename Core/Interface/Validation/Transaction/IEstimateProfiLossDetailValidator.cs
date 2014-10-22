using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Validation
{
    public interface IEstimateProfitLossDetailValidation
    {
        EstimateProfitLossDetail VCreateObject(EstimateProfitLossDetail estimateprofitlossdetail, IEstimateProfitLossDetailService _estimateprofitlossdetailService);
        EstimateProfitLossDetail VUpdateObject(EstimateProfitLossDetail estimateprofitlossdetail, IEstimateProfitLossDetailService _estimateprofitlossdetailService);
    }
}
