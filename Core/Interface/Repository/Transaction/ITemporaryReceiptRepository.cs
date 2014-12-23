using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface ITemporaryReceiptRepository : IRepository<TemporaryReceipt>
    { 
       IQueryable<TemporaryReceipt> GetQueryable();
       TemporaryReceipt GetObjectById(int Id);
       TemporaryReceipt CreateObject(TemporaryReceipt model);
       TemporaryReceipt UpdateObject(TemporaryReceipt model);
       TemporaryReceipt SoftDeleteObject(TemporaryReceipt model);
       int GetLastTRNo(int officeId);
       bool DeleteObject(int Id);
    }
}