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
    public class TemporaryReceiptJobValidation : ITemporaryReceiptJobValidation
    {  
        
        public TemporaryReceiptJob VCreateObject(TemporaryReceiptJob temporaryReceiptJob, ITemporaryReceiptJobService _temporaryReceiptJobService)
        {
            return temporaryReceiptJob;
        }

        public TemporaryReceiptJob VUpdateObject(TemporaryReceiptJob temporaryReceiptJob, ITemporaryReceiptJobService _temporaryReceiptJobService)
        { 
            return temporaryReceiptJob;
        }

        public bool isValid(TemporaryReceiptJob obj)
        { 
            bool isValid = !obj.Errors.Any();
            return isValid;
        }
    }
}
