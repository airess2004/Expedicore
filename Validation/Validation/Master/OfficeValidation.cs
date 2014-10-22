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
    public class OfficeValidation : IOfficeValidation
    {  
        public Office VName(Office office, IOfficeService _officeService)
        {
            if (String.IsNullOrEmpty(office.Name) || office.Name.Trim() == "")
            {
                office.Errors.Add("Name", "Tidak boleh kosong");
            }
            if (_officeService.IsNameDuplicated(office))
            {
                office.Errors.Add("Name", "Tidak boleh diduplikasi");
            }
            return office;
        }

        public Office VAbbrevation(Office office, IOfficeService _officeService)
        {
            if (String.IsNullOrEmpty(office.Abbrevation) || office.Abbrevation.Trim() == "")
            {
                office.Errors.Add("Abbrevation", "Tidak boleh kosong");
            }
            return office;
        }
        
        public Office VCreateObject(Office office, IOfficeService _officeService)
        {
            VName(office, _officeService);
            if (!isValid(office)) { return office; }
            VAbbrevation(office, _officeService);
            if (!isValid(office)) { return office; }
            return office;
        }

        public Office VUpdateObject(Office office, IOfficeService _officeService)
        { 
            VName(office, _officeService);
            if (!isValid(office)) { return office; }
            VAbbrevation(office, _officeService);
            if (!isValid(office)) { return office; }
            return office;
        }

        public bool isValid(Office obj)
        { 
            bool isValid = !obj.Errors.Any();
            return isValid;
        }
    }
}
