using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface ICashAdvanceRepository : IRepository<CashAdvance>
    { 
       IQueryable<CashAdvance> GetQueryable();
       CashAdvance GetObjectById(int Id);
       CashAdvance CreateObject(CashAdvance model);
       CashAdvance UpdateObject(CashAdvance model);
       CashAdvance SoftDeleteObject(CashAdvance model);
       bool DeleteObject(int Id);
       int GetCashAdvanceNo(int officeId);
    }
}