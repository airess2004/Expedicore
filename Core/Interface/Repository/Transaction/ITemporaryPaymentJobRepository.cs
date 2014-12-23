using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface ITemporaryPaymentJobRepository : IRepository<TemporaryPaymentJob>
    { 
       IQueryable<TemporaryPaymentJob> GetQueryable();
       TemporaryPaymentJob GetObjectById(int Id);
       TemporaryPaymentJob CreateObject(TemporaryPaymentJob model);
       TemporaryPaymentJob UpdateObject(TemporaryPaymentJob model);
       TemporaryPaymentJob SoftDeleteObject(TemporaryPaymentJob model);
       bool DeleteObject(int Id);
    }
}