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
    public class TemporaryReceiptValidation : ITemporaryReceiptValidation
    {  
        
        public TemporaryReceipt VCreateObject(TemporaryReceipt temporaryReceipt, ITemporaryReceiptService _temporaryReceiptService)
        {
            return temporaryReceipt;
        }

        public TemporaryReceipt VUpdateObject(TemporaryReceipt temporaryReceipt, ITemporaryReceiptService _temporaryReceiptService)
        { 
            return temporaryReceipt;
        }

        public bool isValid(TemporaryReceipt obj)
        { 
            bool isValid = !obj.Errors.Any();
            return isValid;
        }
    }
}
