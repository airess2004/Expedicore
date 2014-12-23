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
    public class SeaContainerValidation : ISeaContainerValidation
    {  
        
        public SeaContainer VCreateObject(SeaContainer seacontainer, ISeaContainerService _seacontainerService)
        {
            return seacontainer;
        }

        public SeaContainer VUpdateObject(SeaContainer seacontainer, ISeaContainerService _seacontainerService)
        { 
            return seacontainer;
        }

        public bool isValid(SeaContainer obj)
        { 
            bool isValid = !obj.Errors.Any();
            return isValid;
        }
    }
}
