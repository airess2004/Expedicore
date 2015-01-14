using Core.DomainModel;
using Core.Constant;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validation.Validation
{ 
    public class InvoiceValidation : IInvoiceValidation
    {
        public Invoice VCurrency(Invoice invoice)
        {
            if (invoice.CurrencyId != MasterConstant.Currency.IDR && invoice.CurrencyId != MasterConstant.Currency.USD)
            {
                invoice.Errors.Add("CurrencyId", "Invalid Currency");
            }
            return invoice;
        }
        
        public Invoice VIsDeleted(Invoice invoice)
        { 
            if (invoice.IsDeleted == true)
            {
                invoice.Errors.Add("Generic", "Invoice Is Deleted");
            }
            return invoice;
        }

        public Invoice VIsUnConfirmed(Invoice invoice)
        { 
            if (invoice.IsConfirmed == false)
            {
                invoice.Errors.Add("Generic", "Invoice Is Not Confirmed");
            }
            return invoice;
        }
         
        public Invoice VIsNotPrinted(Invoice invoice)
        {
            if (invoice.Printing < 1 || invoice.Printing == null)
            {
                invoice.Errors.Add("Generic", "Invoice Is Not Printed");
            }
            return invoice;
        }

        public Invoice VIsConfirmed(Invoice invoice)
        { 
            if (invoice.IsConfirmed == true)
            {
                invoice.Errors.Add("Generic", "Invoice Is Confirmed");
            }
            return invoice;
        }

        public Invoice VvalidInvoiceUpdate(Invoice invoice, IInvoiceService _invoiceService)
        { 
            Invoice existInvoice = _invoiceService.GetObjectById(invoice.Id);
            if (existInvoice == null)
            {
                invoice.Errors.Add("Generic", "Invalid Invoice");
            }
            else
            {
                if (existInvoice.OfficeId != invoice.OfficeId)
                {
                    invoice.Errors.Add("Generic", "Invalid Invoice");
                    return invoice;
                }
                if (existInvoice.Paid.HasValue && existInvoice.Paid.Value == true)
                {
                    invoice.Errors.Add("Generic", "Invoice has been paid");
                    return invoice;
                }
                if (existInvoice.IsDeleted == true)
                {
                    invoice.Errors.Add("Generic", "Invoice has been deleted");
                    return invoice;
                }
                if (existInvoice.LinkTo != null && existInvoice.LinkTo != "" && existInvoice.LinkTo.Substring(0, 6).ToLower() == "cancel")
                {
                    invoice.Errors.Add("Generic", "Cannot update Cancellation Invoice");
                    return invoice;
                }
                if (existInvoice.IsConfirmed == true)
                {
                    invoice.Errors.Add("Generic", "Invoice has been Confirmed");
                    return invoice;
                }
            }
            return invoice;
        }
         
        public Invoice VvalidInvoiceDelete(Invoice invoice, IInvoiceService _invoiceService)
        {
            Invoice existInvoice = _invoiceService.GetObjectById(invoice.Id);
            if (existInvoice == null)
            {
                invoice.Errors.Add("Generic", "Invalid Invoice");
            }
            else
            {
                if (existInvoice.OfficeId != invoice.OfficeId)
                {
                    invoice.Errors.Add("Generic", "Invalid Invoice");
                    return invoice;
                }
                if (existInvoice.IsDeleted == true)
                {
                    invoice.Errors.Add("Generic", "Invoice has been deleted");
                    return invoice;
                }
            }
            return invoice;
        }

        public Invoice VvalidShipmentOrder(Invoice invoice, IShipmentOrderService _shipmentOrderService)
        { 
            ShipmentOrder shipmentOrder =  _shipmentOrderService.GetObjectById(invoice.ShipmentOrderId);
            if (shipmentOrder == null)
            {
                invoice.Errors.Add("ShipmentOrder", "Invalid ShipmentOrder");
            }
            else
            {
                if (shipmentOrder.OfficeId != invoice.OfficeId)
                {
                    invoice.Errors.Add("ShipmentOrder", "Invalid ShipmentOrder");
                }
            }
            return invoice;
        }

        public Invoice VhasInvoiceDetail(Invoice invoice, IInvoiceDetailService _invoicelDetailService)
        {
            InvoiceDetail existInvoiceDetail = _invoicelDetailService.GetQueryable().Where(x => x.InvoiceId == invoice.Id).FirstOrDefault();
            if (existInvoiceDetail == null)
            {
                invoice.Errors.Add("Generic", "Tidak mempunyai detail");
            }
            return invoice;
        }

        public Invoice VReceivableHasNoOtherAssociation(Invoice invoice,IReceivableService _receivableService,IReceiptVoucherDetailService _receiptVoucherDetailService,IInvoiceDetailService _invoiceDetailService)
        {
            List<InvoiceDetail> invoiceDetail = _invoiceDetailService.GetQueryable().Where(x => x.InvoiceId == invoice.Id && x.IsDeleted == false).ToList();
            foreach (var item in invoiceDetail)
            {
                Receivable receivable = _receivableService.GetObjectBySource(MasterConstant.SourceDocument.Invoice, invoice.Id,item.Id);
                ReceiptVoucherDetail receiptVoucherDetail = _receiptVoucherDetailService.GetObjectsByReceivableId(receivable.Id).FirstOrDefault();
                if (receiptVoucherDetail != null)
                {
                    invoice.Errors.Add("Generic", "Sales Invoice Sudah di Buat ReceiptVoucher : " + receiptVoucherDetail.ReceiptVoucherId);
                    return invoice;
                }
            }
            return invoice;
        }

        public Invoice VCreateObject(Invoice invoice, IShipmentOrderService _shipmentOrderService)
        {
            VvalidShipmentOrder(invoice, _shipmentOrderService);
            if (!isValid(invoice)) { return invoice; }
            VCurrency(invoice);
            if (!isValid(invoice)) { return invoice; }
            return invoice;
        }

        public Invoice VUpdateObject(Invoice invoice, IInvoiceService _invoiceService,IShipmentOrderService _shipmentOrderService)
        {
            VvalidInvoiceUpdate(invoice, _invoiceService);
            if (!isValid(invoice)) { return invoice; }
            VvalidShipmentOrder(invoice, _shipmentOrderService);
            if (!isValid(invoice)) { return invoice; }
            VCurrency(invoice);
            if (!isValid(invoice)) { return invoice; }
            return invoice;
        }
         
        public Invoice VSoftDeleteObject(Invoice invoice, IInvoiceService _invoiceService)
        {
            VvalidInvoiceDelete(invoice, _invoiceService);
            if (!isValid(invoice)) { return invoice; }
            VIsConfirmed(invoice);
            if (!isValid(invoice)) { return invoice; }
            VIsDeleted(invoice);
            if (!isValid(invoice)) { return invoice; }
            return invoice;
        }

        public Invoice VConfirmObject(Invoice invoice ,IInvoiceDetailService _invoiceDetailService)
        {
            VIsConfirmed(invoice);
            if (!isValid(invoice)) { return invoice; }
            VIsDeleted(invoice);
            if (!isValid(invoice)) { return invoice; }
            VhasInvoiceDetail(invoice, _invoiceDetailService);
            if (!isValid(invoice)) { return invoice; }
            VIsNotPrinted(invoice);
            if (!isValid(invoice)) { return invoice; }
            return invoice;
        }

        public Invoice VUnConfirmObject(Invoice invoice,IReceivableService _receivableService,
            IReceiptVoucherDetailService _receiptVoucherDetailService,IInvoiceDetailService _invoiceDetailService)
        {
            VIsUnConfirmed(invoice);
            if (!isValid(invoice)) { return invoice; }
            VIsDeleted(invoice);
            if (!isValid(invoice)) { return invoice; }
            VReceivableHasNoOtherAssociation(invoice, _receivableService, _receiptVoucherDetailService,_invoiceDetailService);
            if (!isValid(invoice)) { return invoice; }
            return invoice;
        }
         
        public Invoice VPrint(Invoice invoice)
        {
            VIsConfirmed(invoice);
            if (!isValid(invoice)) { return invoice; }
            VIsDeleted(invoice);
            if (!isValid(invoice)) { return invoice; }
            return invoice;
        }

        public bool isValid(Invoice obj)
        { 
            bool isValid = !obj.Errors.Any();
            return isValid;
        }
    }
}
