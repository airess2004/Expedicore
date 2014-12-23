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
        InvoiceDetail CreateObject(InvoiceDetail invDetail, IInvoiceService _invoiceService);
        InvoiceDetail ConfirmObject(InvoiceDetail invoiceDetail);
        InvoiceDetail UnconfirmObject(InvoiceDetail invoiceDetail);
        InvoiceDetail UpdateObject(InvoiceDetail invoiceDetail, IInvoiceService _invoiceService, IInvoiceDetailService _invoiceDetailService);
        InvoiceDetail SoftDeleteObject(InvoiceDetail invoiceDetail, IInvoiceService _invoiceService, IInvoiceDetailService _invoiceDetailService);
    }
}