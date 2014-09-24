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
            if (_continentService.IsNameDuplicate(continent))
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
    }
}
