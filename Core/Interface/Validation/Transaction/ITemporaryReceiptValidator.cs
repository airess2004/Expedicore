using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Validation
{
    public interface ITemporaryReceiptValidation
    {
        TemporaryReceipt VCreateObject(TemporaryReceipt temporaryReceipt, ITemporaryReceiptService _temporaryReceiptService);
        TemporaryReceipt VUpdateObject(TemporaryReceipt temporaryReceipt, ITemporaryReceiptService _temporaryReceiptService);
    }
}
