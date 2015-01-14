using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IInvoiceService
    {
        IQueryable<Invoice> GetQueryable();
        Invoice GetObjectById(int Id);
        Invoice GetObjectByShipmentOrderId(int Id);
        Invoice CreateObject(Invoice invoice, IShipmentOrderService _shipmentOrderService,IContactService _contactService, 
                             IExchangeRateService _exchangeRateService);
        Invoice UpdateObject(Invoice invoice, IShipmentOrderService _shipmentOrderService);
        Invoice SoftDeleteObject(Invoice invoice, IInvoiceDetailService _invoiceDetailService);
        Invoice CalculateTotalInvoice(Invoice invoice, IInvoiceDetailService _invoiceDetailService);
        Invoice ConfirmObject(Invoice invoice, DateTime confirmationDate, IInvoiceDetailService _invoiceDetailService,
            IReceivableService _receiveableService);
        Invoice UnconfirmObject(Invoice invoice, DateTime confirmationDate, IInvoiceDetailService _invoiceDetailService,
            IReceivableService _receivableService, IReceiptVoucherDetailService _receiptVoucherDetailService);
        Invoice CalculateTotalUSDIDR(int invoiceId, IInvoiceDetailService _invoiceDetailService);
        Invoice Paid(Invoice invoice);
        Invoice Unpaid(Invoice invoice);
        Invoice Print(int Id, string fd);
    }
}