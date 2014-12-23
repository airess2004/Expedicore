using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validation.Validation
{ 
    public class InvoiceDetailValidation : IInvoiceDetailValidation
    {  
        public InvoiceDetail VvalidInvoice(InvoiceDetail invoiceDetail, IInvoiceService _invoiceService)
        { 
            Invoice existInvoice = _invoiceService.GetObjectById(invoiceDetail.InvoiceId);
            if (existInvoice != null)
            {
                invoiceDetail.Errors.Add("Generic", "Invalid Invoice");
            }
            else
            {
                if (existInvoice.OfficeId != invoiceDetail.OfficeId)
                {
                    invoiceDetail.Errors.Add("Generic", "Invalid Invoice");
                    return invoiceDetail;
                }
                if (existInvoice.Paid.HasValue && existInvoice.Paid.Value == true)
                {
                    invoiceDetail.Errors.Add("Generic", "Invoice has been paid");
                    return invoiceDetail;
                }
                if (existInvoice.IsDeleted == true)
                {
                    invoiceDetail.Errors.Add("Generic", "Invoice has been deleted");
                    return invoiceDetail;
                }
                if (existInvoice.LinkTo != null && existInvoice.LinkTo != "" && existInvoice.LinkTo.Substring(0, 6).ToLower() == "cancel")
                {
                    invoiceDetail.Errors.Add("Generic", "Cannot update Cancellation Invoice");
                    return invoiceDetail;
                }
                if (existInvoice.IsConfirmed == true)
                {
                    invoiceDetail.Errors.Add("Generic", "Invoice has been Confirmed");
                    return invoiceDetail;
                }
            }
            return invoiceDetail;
        }

        public InvoiceDetail VValidInvoiceDetail(InvoiceDetail invoiceDetail, IInvoiceDetailService _invoiceDetailService)
        {
            InvoiceDetail existInvoiceDetail = _invoiceDetailService.GetObjectById(invoiceDetail.Id);
            if (existInvoiceDetail == null)
            {
                invoiceDetail.Errors.Add("Generic", "Invalid Invoice Detail");
                return invoiceDetail;
            }
            return invoiceDetail;
        }
        public InvoiceDetail VCreateObject(InvoiceDetail invoiceDetail, IInvoiceService _invoiceService)
        { 
            VvalidInvoice(invoiceDetail,_invoiceService);
            if (!isValid(invoiceDetail)) { return invoiceDetail; }
            return invoiceDetail;
        }

        public InvoiceDetail VUpdateObject(InvoiceDetail invoiceDetail, IInvoiceService _invoiceService,IInvoiceDetailService _invoiceDetailService)
        {
            VvalidInvoice(invoiceDetail, _invoiceService);
            if (!isValid(invoiceDetail)) { return invoiceDetail; }
            VValidInvoiceDetail(invoiceDetail, _invoiceDetailService);
            if (!isValid(invoiceDetail)) { return invoiceDetail; }
            return invoiceDetail;
        }
         
        public InvoiceDetail VSoftDeleteObject(InvoiceDetail invoiceDetail, IInvoiceService _invoiceService, IInvoiceDetailService _invoiceDetailService)
        {
            VvalidInvoice(invoiceDetail, _invoiceService);
            if (!isValid(invoiceDetail)) { return invoiceDetail; }
            VValidInvoiceDetail(invoiceDetail, _invoiceDetailService);
            if (!isValid(invoiceDetail)) { return invoiceDetail; }
            return invoiceDetail;
        }

        public bool isValid(InvoiceDetail obj)
        { 
            bool isValid = !obj.Errors.Any();
            return isValid;
        }
    }
}
