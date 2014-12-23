using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Validation
{
    public interface IInvoiceDetailValidation
    {
        InvoiceDetail VCreateObject(InvoiceDetail invoiceDetail, IInvoiceService _invoiceService);
        InvoiceDetail VUpdateObject(InvoiceDetail invoiceDetail, IInvoiceService _invoiceService, IInvoiceDetailService _invoiceDetailService);
        InvoiceDetail VSoftDeleteObject(InvoiceDetail invoiceDetail, IInvoiceService _invoiceService, IInvoiceDetailService _invoiceDetailService);
    }
}
