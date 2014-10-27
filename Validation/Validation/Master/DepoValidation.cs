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
    public class DepoValidation : IDepoValidation
    {  
        public Depo VName(Depo depo, IDepoService _depoService)
        {
            if (String.IsNullOrEmpty(depo.Name) || depo.Name.Trim() == "")
            {
                depo.Errors.Add("Name", "Tidak boleh kosong");
            }
            else if (_depoService.IsNameDuplicated(depo))
            {
                depo.Errors.Add("Name", "Tidak boleh diduplikasi");
            }
            return depo;
        }

        
        public Depo VCreateObject(Depo depo, IDepoService _depoService)
        {
            VName(depo, _depoService);
            if (!isValid(depo)) { return depo; }
            return depo;
        }

        public Depo VUpdateObject(Depo depo, IDepoService _depoService)
        {
            VObject(depo, _depoService);
            if (!isValid(depo)) { return depo; }
            VName(depo, _depoService);
            if (!isValid(depo)) { return depo; }
            return depo;
        }

        public bool isValid(Depo obj)
        { 
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public Depo VObject(Depo depo, IDepoService _depoService)
        {
            Depo olddepo = _depoService.GetObjectById(depo.Id);
            if (olddepo == null)
            {
                depo.Errors.Add("Generic", "Invalid Data For Update");
            }
            else if (!VOffice(depo.OfficeId, olddepo.OfficeId))
            {
                depo.Errors.Add("Generic", "Invalid Data For Update");
            }
            return depo;
        }

        public bool VOffice(int OfficeId, int CekOfficeId)
        {
            return OfficeId == CekOfficeId;
        }

    }
}
