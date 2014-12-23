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
    public class TemporaryPaymentJobValidation : ITemporaryPaymentJobValidation
    {  
        
        public TemporaryPaymentJob VCreateObject(TemporaryPaymentJob temporaryPaymentJob, ITemporaryPaymentJobService _temporaryPaymentJobService)
        {
            return temporaryPaymentJob;
        }

        public TemporaryPaymentJob VUpdateObject(TemporaryPaymentJob temporaryPaymentJob, ITemporaryPaymentJobService _temporaryPaymentJobService)
        { 
            return temporaryPaymentJob;
        }

        public bool isValid(TemporaryPaymentJob obj)
        { 
            bool isValid = !obj.Errors.Any();
            return isValid;
        }
    }
}
