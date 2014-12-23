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
    public class VatValidation : IVatValidation
    {  
        public Vat VName(Vat vat, IVatService _vatService)
        {
            if (String.IsNullOrEmpty(vat.Name) || vat.Name.Trim() == "")
            {
                vat.Errors.Add("Name", "Tidak boleh kosong");
            }
            else if (_vatService.IsNameDuplicated(vat))
            {
                vat.Errors.Add("Name", "Tidak boleh diduplikasi");
            }
            return vat;
        }

        
        public Vat VCreateObject(Vat vat, IVatService _vatService)
        {
            VName(vat, _vatService);
            if (!isValid(vat)) { return vat; }
            return vat;
        }

        public Vat VUpdateObject(Vat vat, IVatService _vatService)
        {
            VObject(vat, _vatService);
            if (!isValid(vat)) { return vat; }
            VName(vat, _vatService);
            if (!isValid(vat)) { return vat; }
            return vat;
        }
       
        public Vat VObject(Vat vat, IVatService _vatService)
        {
            Vat oldvat = _vatService.GetObjectById(vat.Id);
            if (oldvat == null)
            {
                vat.Errors.Add("Generic", "Invalid Data For Update");
            }
            else if (!VOffice(vat.OfficeId,oldvat.OfficeId))
            {
                vat.Errors.Add("Generic", "Invalid Data For Update");
            }
            return vat;
        }


        public bool VOffice(int OfficeId, int CekOfficeId)
        {
            return OfficeId == CekOfficeId;
        }

        public bool isValid(Vat obj)
        { 
            bool isValid = !obj.Errors.Any();
            return isValid;
        }
    }
}
