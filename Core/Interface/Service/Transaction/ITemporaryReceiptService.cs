using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface ITemporaryReceiptService
    {
        IQueryable<TemporaryReceipt> GetQueryable();
        TemporaryReceipt GetObjectById(int Id);
        TemporaryReceipt CreateObject(TemporaryReceipt temporaryReceipt, IExchangeRateService _exchangeRateService);
        TemporaryReceipt UpdateObject(TemporaryReceipt temporaryReceipt); 
        TemporaryReceipt SoftDeleteObject(TemporaryReceipt temporaryReceipt);
    }
}