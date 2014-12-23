using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface ICashSettlementRepository : IRepository<CashSettlement>
    { 
       IQueryable<CashSettlement> GetQueryable();
       CashSettlement GetObjectById(int Id);
       CashSettlement CreateObject(CashSettlement model);
       CashSettlement UpdateObject(CashSettlement model);
       CashSettlement SoftDeleteObject(CashSettlement model);
       bool DeleteObject(int Id);
       int GetCashSettlementNo(int officeId);
    }
}