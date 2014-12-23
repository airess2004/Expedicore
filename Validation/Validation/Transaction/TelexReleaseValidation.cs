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
    public class TelexReleaseValidation : ITelexReleaseValidation
    {  
        
        public TelexRelease VCreateObject(TelexRelease telexRelease, ITelexReleaseService _telexReleaseService)
        {
            return telexRelease;
        }

        public TelexRelease VUpdateObject(TelexRelease telexRelease, ITelexReleaseService _telexReleaseService)
        { 
            return telexRelease;
        }

        public bool isValid(TelexRelease obj)
        { 
            bool isValid = !obj.Errors.Any();
            return isValid;
        }
    }
}
