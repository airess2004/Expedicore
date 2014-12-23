using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;
using Core.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class InvoiceDetailService : IInvoiceDetailService 
    {  
        private IInvoiceDetailRepository _repository;
        private IInvoiceDetailValidation _validator;

        public InvoiceDetailService(IInvoiceDetailRepository _invoiceDetailRepository, IInvoiceDetailValidation _invoiceDetailValidation)
        {
            _repository = _invoiceDetailRepository;
            _validator = _invoiceDetailValidation;
        }

        public IQueryable<InvoiceDetail> GetQueryable()
        {  
            return _repository.GetQueryable();
        }

        public InvoiceDetail GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public InvoiceDetail ConfirmObject(InvoiceDetail invoiceDetail)
        {
            invoiceDetail = _repository.ConfirmObject(invoiceDetail);
            return invoiceDetail;
        }

        public InvoiceDetail UnconfirmObject(InvoiceDetail invoiceDetail)
        {
            invoiceDetail = _repository.UnconfirmObject(invoiceDetail);
            return invoiceDetail;
        }


        public InvoiceDetail CreateObject(InvoiceDetail invDetail,IInvoiceService _invoiceService)
        {
            invDetail.Errors = new Dictionary<String, String>();
            
            if (isValid(_validator.VCreateObject(invDetail, _invoiceService)))
            {
                InvoiceDetail newInvDetail = new InvoiceDetail();
                newInvDetail.CostId = invDetail.CostId;
                newInvDetail.Amount = invDetail.Amount;
                newInvDetail.AmountCrr = invDetail.AmountCrr;
                newInvDetail.CodingQuantity = invDetail.CodingQuantity;
                newInvDetail.OfficeId = invDetail.OfficeId;
                newInvDetail.CreatedById = invDetail.CreatedById;
                newInvDetail.CreatedAt = DateTime.Today;
                newInvDetail.DebetCredit = _invoiceService.GetObjectById(invDetail.InvoiceId).DebetCredit;
                newInvDetail.Description = !String.IsNullOrEmpty(invDetail.Description) ? invDetail.Description.ToUpper() : "";
                newInvDetail.PerQty = invDetail.PerQty;
                newInvDetail.InvoiceId = invDetail.InvoiceId;
                newInvDetail.Quantity = invDetail.Quantity;
                newInvDetail.Sign = invDetail.Sign;
                newInvDetail.Type = invDetail.Type;
                newInvDetail.VatId = invDetail.VatId;
                newInvDetail.AmountVat = (invDetail.Amount * invDetail.PercentVat) / 100;
                newInvDetail.PercentVat = invDetail.PercentVat;
                newInvDetail.EPLDetailId = invDetail.EPLDetailId;
                newInvDetail = _repository.CreateObject(newInvDetail);
                Invoice invoice = _invoiceService.GetObjectById(newInvDetail.InvoiceId);
                invoice = _invoiceService.CalculateTotalInvoice(invoice, this);
            }
            return invDetail;
        }

        public InvoiceDetail UpdateObject(InvoiceDetail invoiceDetail,IInvoiceService _invoiceService,IInvoiceDetailService _invoiceDetailService)
        {
            if (isValid(_validator.VUpdateObject(invoiceDetail,_invoiceService,_invoiceDetailService)))
            {
                invoiceDetail.Description = !String.IsNullOrEmpty(invoiceDetail.Description) ? invoiceDetail.Description.ToUpper() : "";
                invoiceDetail.AmountVat = (invoiceDetail.Amount * invoiceDetail.PercentVat) / 100;
                invoiceDetail = _repository.UpdateObject(invoiceDetail);
                Invoice invoice = _invoiceService.GetObjectById(invoiceDetail.InvoiceId);
                invoice = _invoiceService.CalculateTotalInvoice(invoice, this);
            }
            return invoiceDetail;
        }

        public InvoiceDetail SoftDeleteObject(InvoiceDetail invoiceDetail,IInvoiceService _invoiceService,IInvoiceDetailService _invoiceDetailService)
        {
            if (isValid(_validator.VSoftDeleteObject(invoiceDetail,_invoiceService,_invoiceDetailService)))
            {
                invoiceDetail = _repository.SoftDeleteObject(invoiceDetail);
                Invoice invoice = _invoiceService.GetObjectById(invoiceDetail.InvoiceId);
                invoice = _invoiceService.CalculateTotalInvoice(invoice, this);
            }
            return invoiceDetail;
        }


        public bool isValid(InvoiceDetail obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }
    }
}
