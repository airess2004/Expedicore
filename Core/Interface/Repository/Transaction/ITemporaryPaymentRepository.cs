using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface ITemporaryPaymentRepository : IRepository<TemporaryPayment>
    { 
       IQueryable<TemporaryPayment> GetQueryable();
       TemporaryPayment GetObjectById(int Id);
       TemporaryPayment CreateObject(TemporaryPayment model);
       TemporaryPayment UpdateObject(TemporaryPayment model);
       TemporaryPayment SoftDeleteObject(TemporaryPayment model);
       int GetLastTPNo(int officeId);
       bool DeleteObject(int Id);
    }
}