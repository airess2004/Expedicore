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
    public class TemporaryPaymentValidation : ITemporaryPaymentValidation
    {  
        
        public TemporaryPayment VCreateObject(TemporaryPayment temporaryPayment, ITemporaryPaymentService _temporaryPaymentService)
        {
            return temporaryPayment;
        }

        public TemporaryPayment VUpdateObject(TemporaryPayment temporaryPayment, ITemporaryPaymentService _temporaryPaymentService)
        { 
            return temporaryPayment;
        }

        public bool isValid(TemporaryPayment obj)
        { 
            bool isValid = !obj.Errors.Any();
            return isValid;
        }
    }
}
