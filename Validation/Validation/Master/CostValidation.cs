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
    public class CostValidation : ICostValidation
    {  
        public Cost VName(Cost cost, ICostService _costService)
        {
            if (String.IsNullOrEmpty(cost.Name) || cost.Name.Trim() == "")
            {
                cost.Errors.Add("Name", "Tidak boleh kosong");
            }
            else if (_costService.IsNameDuplicated(cost))
            {
                cost.Errors.Add("Name", "Tidak boleh diduplikasi");
            }
            return cost;
        }
       
        //public Cost VRemarks(Cost cost)
        //{
        //    if (String.IsNullOrEmpty(cost.Remarks) || cost.Remarks.Trim() == "")
        //    {
        //        cost.Errors.Add("Remarks", "Tidak boleh kosong");
        //    }
        //    return cost;
        //}

        public Cost VCreateObject(Cost cost, ICostService _costService)
        {
            VName(cost, _costService);
            if (!isValid(cost)) { return cost; }
            //VRemarks(cost);
            //if (!isValid(cost)) { return cost; }
            return cost;
        }

        public Cost VUpdateObject(Cost cost, ICostService _costService)
        {
            VObject(cost, _costService);
            if (!isValid(cost)) { return cost; }
            VName(cost, _costService);
            if (!isValid(cost)) { return cost; }
            //VRemarks(cost);
            //if (!isValid(cost)) { return cost; }
            return cost;
        }

        public bool isValid(Cost obj)
        { 
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public Cost VObject(Cost cost, ICostService _costService)
        {
            Cost oldcost = _costService.GetObjectById(cost.Id);
            if (oldcost == null)
            {
                cost.Errors.Add("Generic", "Invalid Data For Update");
            }
            else if (!VOffice(cost.OfficeId, oldcost.OfficeId))
            {
                cost.Errors.Add("Generic", "Invalid Data For Update");
            }
            return cost;
        }

        public bool VOffice(int OfficeId, int CekOfficeId)
        {
            return OfficeId == CekOfficeId;
        }

    }
}
