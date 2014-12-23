using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface ICashSettlementService
    {
        IQueryable<CashSettlement> GetQueryable();
        CashSettlement GetObjectById(int Id);
        CashSettlement CreateObject(CashSettlement cashSettlement);
        CashSettlement UpdateObject(CashSettlement cashSettlement);
        CashSettlement SoftDeleteObject(CashSettlement cashSettlement);
    }
}