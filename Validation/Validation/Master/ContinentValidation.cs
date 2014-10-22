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
    public class ContinentValidation : IContinentValidation
    {  
        public Continent VName(Continent continent, IContinentService _continentService)
        {
            if (String.IsNullOrEmpty(continent.Name) || continent.Name.Trim() == "")
            {
                continent.Errors.Add("Name", "Tidak boleh kosong");
            }
            else if (_continentService.IsNameDuplicated(continent))
            {
                continent.Errors.Add("Name", "Tidak boleh diduplikasi");
            }
            return continent;
        }

        public Continent VAbbrevation(Continent continent, IContinentService _continentService)
        {
            if (String.IsNullOrEmpty(continent.Abbrevation) || continent.Abbrevation.Trim() == "")
            {
                continent.Errors.Add("Abbrevation", "Tidak boleh kosong");
            }
            return continent;
        }
        
        public Continent VCreateObject(Continent continent, IContinentService _continentService)
        {
            VName(continent, _continentService);
            if (!isValid(continent)) { return continent; }
            VAbbrevation(continent, _continentService);
            if (!isValid(continent)) { return continent; }
            return continent;
        }

        public Continent VUpdateObject(Continent continent, IContinentService _continentService)
        {
            VObject(continent, _continentService);
            if (!isValid(continent)) { return continent; }
            VName(continent, _continentService);
            if (!isValid(continent)) { return continent; }
            VAbbrevation(continent, _continentService);
            if (!isValid(continent)) { return continent; }
            return continent;
        }

        public bool isValid(Continent obj)
        { 
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public Continent VObject(Continent continent, IContinentService _continentService)
        {
            Continent oldcontinent = _continentService.GetObjectById(continent.Id);
            if (oldcontinent == null)
            {
                continent.Errors.Add("Generic", "Invalid Data For Update");
            }
            else if (!VOffice(continent.OfficeId, oldcontinent.OfficeId))
            {
                continent.Errors.Add("Generic", "Invalid Data For Update");
            }
            return continent;
        }

        public bool VOffice(int OfficeId, int CekOfficeId)
        {
            return OfficeId == CekOfficeId;
        }

    }
}
