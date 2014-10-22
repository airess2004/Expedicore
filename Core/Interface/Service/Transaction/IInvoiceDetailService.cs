using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IInvoiceDetailService
    {
        IQueryable<InvoiceDetail> GetQueryable();
        InvoiceDetail GetObjectById(int Id);
        InvoiceDetail GetObjectByShipmentOrderId(int Id);
        InvoiceDetail CreateUpdateObject(InvoiceDetail invoicedetail);
        InvoiceDetail CreateObject(InvoiceDetail invoicedetail);
        InvoiceDetail UpdateObject(InvoiceDetail invoicedetail); 
        InvoiceDetail SoftDeleteObject(InvoiceDetail invoicedetail);
    }
}