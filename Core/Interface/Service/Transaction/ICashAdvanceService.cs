using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface ICashAdvanceService
    {
        IQueryable<CashAdvance> GetQueryable();
        CashAdvance GetObjectById(int Id);
        CashAdvance CreateObject(CashAdvance cashAdvance, IShipmentOrderService _shipmentOrderService, IContactService _contactService);
        CashAdvance UpdateObject(CashAdvance cashadvance);
        CashAdvance SoftDeleteObject(CashAdvance cashAdvance, ICashAdvanceDetailService _cashAdvanceDetailService);
    }
}