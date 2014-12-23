using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface ICashAdvanceDetailService
    {
        IQueryable<CashAdvanceDetail> GetQueryable();
        CashAdvanceDetail GetObjectById(int Id);
        CashAdvanceDetail CreateObject(CashAdvanceDetail cashAdvanceDetail, IShipmentOrderService _shipmentOrderService
            , ICashAdvanceService _cashAdvanceService);
        CashAdvanceDetail UpdateObject(CashAdvanceDetail cashAdvanceDetail, ICashAdvanceService _cashAdvanceService);
        CashAdvanceDetail SoftDeleteObject(CashAdvanceDetail cashadvancedetail);
    }
}