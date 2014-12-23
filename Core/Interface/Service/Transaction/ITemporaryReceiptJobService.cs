using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface ITemporaryReceiptJobService
    {
        IQueryable<TemporaryReceiptJob> GetQueryable();
        TemporaryReceiptJob GetObjectById(int Id);
        TemporaryReceiptJob CreateObject(TemporaryReceiptJob temporaryReceiptJob, ITemporaryReceiptService _temporaryReceiptService);
        TemporaryReceiptJob UpdateObject(TemporaryReceiptJob temporaryReceiptJob); 
        TemporaryReceiptJob SoftDeleteObject(TemporaryReceiptJob temporaryReceiptJob);
    }
}