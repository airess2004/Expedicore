using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Validation
{
    public interface IInvoiceValidation
    {
        Invoice VCreateObject(Invoice invoicedetail, IInvoiceService _invoicedetailService);
        Invoice VUpdateObject(Invoice invoicedetail, IInvoiceService _invoicedetailService);
    }
}
