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
        public EstimateProfitLossDetail VvalidEPLDetail(EstimateProfitLossDetail estimateprofitlossdetail, IEstimateProfitLossDetailService _estimateProfitLossDetailService)
        {
            EstimateProfitLossDetail epl = _estimateProfitLossDetailService.GetObjectById(estimateprofitlossdetail.EstimateProfitLossId);
            if (epl == null)
            {
                estimateprofitlossdetail.Errors.Add("Generic", "Invalid EPL");
            }
            else
            {
                if (estimateprofitlossdetail.OfficeId != epl.OfficeId)
                {
                    estimateprofitlossdetail.Errors.Add("Generic", "Invalid EPL");
                }
            }
            return estimateprofitlossdetail;
        }

        public EstimateProfitLossDetail VvalidEPL(EstimateProfitLossDetail estimateprofitlossdetail, IEstimateProfitLossService _estimateprofitlossService)
        {
            EstimateProfitLoss epl = _estimateprofitlossService.GetObjectById(estimateprofitlossdetail.EstimateProfitLossId);
            if (epl == null)
            {
                estimateprofitlossdetail.Errors.Add("Generic", "Invalid EPL");
            }
            else
            {
                if (estimateprofitlossdetail.OfficeId != epl.OfficeId)
                {
                    estimateprofitlossdetail.Errors.Add("Generic", "Invalid EPL");
                }
            }
            return estimateprofitlossdetail;
        }

        public EstimateProfitLossDetail VvalidCost(EstimateProfitLossDetail estimateprofitlossdetail, ICostService _costService)
        { 
            Cost cost = _costService.GetObjectById(estimateprofitlossdetail.CostId);
            if (cost == null)
            {
                estimateprofitlossdetail.Errors.Add("Generic", "Invalid Cost");
            }
            return estimateprofitlossdetail;
        } 

        public EstimateProfitLossDetail VvalidContact(EstimateProfitLossDetail estimateprofitlossdetail, IContactService _contactService)
        {
            Contact cost = _contactService.GetObjectById(estimateprofitlossdetail.ContactId);
            if (cost == null) 
            {
                estimateprofitlossdetail.Errors.Add("Generic", "Invalid Customer");
            }
            return estimateprofitlossdetail;
        }


        public EstimateProfitLossDetail VCreateObject(EstimateProfitLossDetail estimateprofitlossdetail,
            IEstimateProfitLossService _estimateProfitLossService,IContactService _contactService,ICostService _costService)
        {
            VvalidEPL(estimateprofitlossdetail, _estimateProfitLossService);
            if (!isValid(estimateprofitlossdetail)) { return estimateprofitlossdetail; }
            VvalidContact(estimateprofitlossdetail, _contactService);
            if (!isValid(estimateprofitlossdetail)) { return estimateprofitlossdetail; }
            VvalidCost(estimateprofitlossdetail, _costService);
            if (!isValid(estimateprofitlossdetail)) { return estimateprofitlossdetail; }
            return estimateprofitlossdetail;
        }
        
        public EstimateProfitLossDetail VUpdateObject(EstimateProfitLossDetail estimateprofitlossdetail, IEstimateProfitLossService _estimateProfitLossService,
            IEstimateProfitLossDetailService _estimateprofitlossdetailService, IContactService _contactService, ICostService _costService)
        {
            VvalidEPLDetail(estimateprofitlossdetail, _estimateprofitlossdetailService);
            if (!isValid(estimateprofitlossdetail)) { return estimateprofitlossdetail; }
            VvalidEPL(estimateprofitlossdetail, _estimateProfitLossService);
            if (!isValid(estimateprofitlossdetail)) { return estimateprofitlossdetail; }
            VvalidContact(estimateprofitlossdetail, _contactService);
            if (!isValid(estimateprofitlossdetail)) { return estimateprofitlossdetail; }
            VvalidCost(estimateprofitlossdetail, _costService);
            if (!isValid(estimateprofitlossdetail)) { return estimateprofitlossdetail; }
            return estimateprofitlossdetail;
        }

        public EstimateProfitLossDetail VSoftDeleteObject(EstimateProfitLossDetail estimateprofitlossdetail,
            IEstimateProfitLossDetailService _estimateprofitlossdetailService)
        {
            VvalidEPLDetail(estimateprofitlossdetail, _estimateprofitlossdetailService);
            if (!isValid(estimateprofitlossdetail)) { return estimateprofitlossdetail; }
            return estimateprofitlossdetail;
        }

        public bool isValid(EstimateProfitLossDetail obj)
        { 
            bool isValid = !obj.Errors.Any();
            return isValid;
        }
    }
}
