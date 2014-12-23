using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface ITemporaryReceiptJobRepository : IRepository<TemporaryReceiptJob>
    {
        IQueryable<TemporaryReceiptJob> GetQueryable();
        TemporaryReceiptJob GetObjectById(int Id);
        TemporaryReceiptJob CreateObject(TemporaryReceiptJob model);
        TemporaryReceiptJob UpdateObject(TemporaryReceiptJob model);
        TemporaryReceiptJob SoftDeleteObject(TemporaryReceiptJob model);
        bool DeleteObject(int Id);
    }
}