using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface ICashAdvanceDetailRepository : IRepository<CashAdvanceDetail>
    { 
       IQueryable<CashAdvanceDetail> GetQueryable();
       CashAdvanceDetail GetObjectById(int Id);
       CashAdvanceDetail GetObjectByCashAdvanceId(int Id);
       CashAdvanceDetail CreateObject(CashAdvanceDetail model);
       CashAdvanceDetail UpdateObject(CashAdvanceDetail model);
       CashAdvanceDetail SoftDeleteObject(CashAdvanceDetail model);
       CashAdvanceDetail UnconfirmObject(CashAdvanceDetail model);
       CashAdvanceDetail ConfirmObject(CashAdvanceDetail model);
       bool DeleteObject(int Id);
    }
}