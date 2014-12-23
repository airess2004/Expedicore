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
        Invoice VCreateObject(Invoice invoice, IShipmentOrderService _shipmentOrderService);
        Invoice VUpdateObject(Invoice invoice, IInvoiceService _invoiceService, IShipmentOrderService _shipmentOrderService);
        Invoice VSoftDeleteObject(Invoice invoice, IInvoiceService _invoiceService);
        Invoice VConfirmObject(Invoice invoice, IInvoiceDetailService _invoiceDetailService);
        Invoice VUnConfirmObject(Invoice invoice, IReceivableService _receivableService, IReceiptVoucherDetailService _receiptVoucherDetailService);
    }
}
