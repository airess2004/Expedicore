using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Validation
{
    public interface ITemporaryReceiptJobValidation
    {
        TemporaryReceiptJob VCreateObject(TemporaryReceiptJob temporaryReceiptJob, ITemporaryReceiptJobService _temporaryReceiptJobService);
        TemporaryReceiptJob VUpdateObject(TemporaryReceiptJob temporaryReceiptJob, ITemporaryReceiptJobService _temporaryReceiptJobService);
    }
}
