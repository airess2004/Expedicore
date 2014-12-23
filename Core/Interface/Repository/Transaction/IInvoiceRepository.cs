using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IInvoiceRepository : IRepository<Invoice>
    { 
       IQueryable<Invoice> GetQueryable();
       Invoice GetObjectById(int Id);
       Invoice GetObjectByShipmentOrderId(int Id);
       Invoice CreateObject(Invoice model);
       Invoice UpdateObject(Invoice model);
       Invoice UnconfirmObject(Invoice model);
       Invoice ConfirmObject(Invoice model);
       Invoice SoftDeleteObject(Invoice model);
       bool DeleteObject(int Id);
       int GetInvoiceNo(int officeId, string debetCredit);
       int GetNewInvoiceStatus(int officeId, int ShipmentOrderId);
    }
}