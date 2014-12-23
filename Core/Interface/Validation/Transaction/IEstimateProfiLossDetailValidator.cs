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
        EstimateProfitLossDetail VCreateObject(EstimateProfitLossDetail estimateprofitlossdetail,
            IEstimateProfitLossService _estimateProfitLossService, IContactService _contactService, ICostService _costService);
        EstimateProfitLossDetail VUpdateObject(EstimateProfitLossDetail estimateprofitlossdetail, IEstimateProfitLossService _estimateProfitLossService,
           IEstimateProfitLossDetailService _estimateprofitlossdetailService, IContactService _contactService, ICostService _costService);
        EstimateProfitLossDetail VSoftDeleteObject(EstimateProfitLossDetail estimateprofitlossdetail,
            IEstimateProfitLossDetailService _estimateprofitlossdetailService);
    }
}
