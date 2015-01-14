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
    public class InvoiceService : IInvoiceService 
    {  
        private IInvoiceRepository _repository;
        private IInvoiceValidation _validator;

        public InvoiceService(IInvoiceRepository _invoiceRepository, IInvoiceValidation _invoiceValidation)
        {
            _repository = _invoiceRepository;
            _validator = _invoiceValidation;
        }

        public IQueryable<Invoice> GetQueryable()
        {  
            return _repository.GetQueryable();
        }

        public Invoice GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public Invoice GetObjectByShipmentOrderId(int Id)
        {
            return _repository.GetObjectByShipmentOrderId(Id);
        }

        public void CreateInvoiceContraOnEditMode(int InvoiceId,IShipmentOrderService _shipmentOrderService,
                                                    IInvoiceDetailService _invoiceDetailService,IVatService _vatService)
        {
            int newInvNo = 0;
            Invoice existInvoice = new Invoice();
                existInvoice = GetObjectById(InvoiceId);
                if (existInvoice != null)
                {
                    var shipment = _shipmentOrderService.GetObjectById(existInvoice.ShipmentOrderId);
                    string shipmentNo = shipment.ShipmentOrderCode;

                    // Update Existed PR as Deleted and InvoicesEdit to T
                    existInvoice.InvoicesEdit = "T";
                    existInvoice.IsDeleted = true;
                    existInvoice.DeletedAt = DateTime.Today;
                    _repository.UpdateObject(existInvoice);
                    
                    int newInvContraNo = 0;
                    string debetCredit = "";
                    // As Debet
                    if (existInvoice.DebetCredit == MasterConstant.DebetCredit.Debet)
                    {
                        debetCredit = MasterConstant.DebetCredit.Credit;
                        // Get Last PRNo as Contra
                        newInvContraNo = _repository.GetInvoiceNo(existInvoice.OfficeId, debetCredit) + 1;
                    }
                    // As Credit
                    else if (existInvoice.DebetCredit == MasterConstant.DebetCredit.Credit)
                    {
                        debetCredit = MasterConstant.DebetCredit.Debet;
                        // Get Last PRNo as Contra
                        newInvContraNo = _repository.GetInvoiceNo(existInvoice.OfficeId, debetCredit) + 1;
                    }

                    // Get Last InvoiceStatus
                    int invStatusContra = _repository.GetNewInvoiceStatus(existInvoice.OfficeId, existInvoice.ShipmentOrderId);
                   
                    // Create Contra Invoice based on Updated Invoice
                    Invoice invNew = new Invoice();
                   
                    invNew.InvoicesNo = newInvContraNo; 
                    invNew.OfficeId = existInvoice.OfficeId; 
                    invNew.CreatedAt = DateTime.Now;
                    invNew.ShipmentOrderId = existInvoice.ShipmentOrderId;
                    invNew.CreatedById = existInvoice.CreatedById;
                    invNew.Printing = 0;
                    invNew.ContactId = existInvoice.ContactId;
                    invNew.CustomerName = existInvoice.CustomerName;
                    invNew.CustomerAddress = existInvoice.CustomerAddress;
                    invNew.BillId = existInvoice.BillId;
                    invNew.BillName = existInvoice.BillName;
                    invNew.BillAddress = existInvoice.BillAddress;
                    invNew.JenisInvoices = existInvoice.JenisInvoices;
                    invNew.PaymentUSD = existInvoice.PaymentUSD;
                    invNew.PaymentIDR = existInvoice.PaymentIDR;
                    invNew.DebetCredit = debetCredit;
                    invNew.IsDeleted = true;
                    invNew.DeletedAt = DateTime.Now;
                    invNew.CustomerTypeId = existInvoice.CustomerTypeId;
                    invNew.InvoicesEdit = "F";
                    invNew.InvoicesTo = existInvoice.InvoicesTo;
                    invNew.InvoicesAgent = existInvoice.InvoicesAgent;
                    invNew.InvHeader = existInvoice.InvHeader;
                    invNew.Rate = existInvoice.Rate;
                    invNew.ExRateDate = existInvoice.ExRateDate;
                    invNew.TotalVatIDR = existInvoice.TotalVatIDR;
                    invNew.TotalVatUSD = existInvoice.TotalVatUSD;
                    invNew.DueDate = existInvoice.DueDate;
                    invNew.InvoiceStatus = invStatusContra;
                    invNew.LinkTo = "Cancel " + existInvoice.DebetCredit + "N No. " + existInvoice.InvoicesNo;
                    //
                    
                    invNew = _repository.CreateObject(invNew);

                    // Create Contra Invoice Detail based on Updated Invoice
                    List<InvoiceDetail> invDetailList = _invoiceDetailService.GetQueryable().Where(x => x.InvoiceId == existInvoice.Id).ToList();

                    foreach (var item in invDetailList.ToList())
                    {
                        InvoiceDetail newInvoiceDetail = new InvoiceDetail
                        {
                            InvoiceId = invNew.Id,
                            Amount = item.Amount,
                            AmountCrr = item.AmountCrr,
                            AmountVat = item.AmountVat,
                            CodingQuantity = item.CodingQuantity,
                            ConfirmationDate = item.ConfirmationDate,
                            CostId = item.CostId,
                            CreatedAt = DateTime.Today,
                            CreatedById = invNew.CreatedById,
                            DebetCredit = item.DebetCredit, 
                            DeletedAt  = item.DeletedAt,
                            Description = item.Description, 
                            EPLDetailId = item.EPLDetailId,
                            IsConfirmed = item.IsConfirmed, 
                            OfficeId = item.InvoiceId,
                            IsDeleted = item.IsDeleted,
                            PercentVat = item.PercentVat,
                            PerQty = item.PerQty, 
                            Quantity = item.Quantity , 
                            Sequence = item.Sequence ,
                            Sign = item.Sign, 
                            Type = item.Type , 
                            UpdatedAt = item.UpdatedAt ,
                            UpdatedById = item.UpdatedById 
                        };
                        _invoiceDetailService.CreateObject(newInvoiceDetail,this);
                    }
                    // END Create Contra Invoice Detail based on Updated Invoice


                    // ------------------------ Create New Invoice (Duplicate from existing) ------------------------
                    // Get Last invNo
                    newInvNo = _repository.GetInvoiceNo(existInvoice.OfficeId,existInvoice.DebetCredit) + 1;

                    // Get Last InvoiceStatus
                    int invStatus = _repository.GetNewInvoiceStatus(existInvoice.OfficeId,existInvoice.ShipmentOrderId) + 1;

                    // Create New Invoice based on Updated Invoice
                    invNew = new Invoice();
                    invNew.InvoicesNo = newInvNo;
                    invNew.OfficeId = invNew.OfficeId; 
                    invNew.CreatedAt = DateTime.Now;
                    invNew.ShipmentOrderId = existInvoice.ShipmentOrderId;
                    invNew.CreatedById = existInvoice.CreatedById; 
                    invNew.Printing = 0;
                    invNew.ContactId = existInvoice.ContactId;
                    invNew.CustomerName = existInvoice.CustomerName;
                    invNew.CustomerAddress = existInvoice.CustomerAddress;
                    invNew.BillId = existInvoice.BillId;
                    invNew.BillName = existInvoice.BillName;
                    invNew.BillAddress = existInvoice.BillAddress;
                    invNew.JenisInvoices = existInvoice.JenisInvoices;
                    invNew.PaymentUSD = existInvoice.PaymentUSD;
                    invNew.DebetCredit = existInvoice.DebetCredit;
                    invNew.CustomerTypeId = existInvoice.CustomerTypeId;
                    invNew.InvoicesEdit = "F";
                    invNew.InvoicesTo = existInvoice.InvoicesTo;
                    invNew.InvHeader = existInvoice.InvHeader;
                    invNew.InvoicesAgent = existInvoice.InvoicesAgent;
                    invNew.Rate = existInvoice.Rate;
                    invNew.ExRateDate = existInvoice.ExRateDate;
                    invNew.TotalVatIDR = existInvoice.TotalVatIDR;
                    invNew.DueDate = existInvoice.DueDate;
                    invNew.LinkTo = "Edit " + existInvoice.DebetCredit + "N No. " + existInvoice.InvoicesNo;

                    // Increment after Contra
                    invNew.InvoiceStatus = invStatus;

                    //
                    invNew = _repository.CreateObject(invNew);

                    // Set Invoice Format
                    // Create New Invoice Detail based on Updated Invoice
                    foreach (var item in invDetailList.ToList())
                    {
                        item.InvoiceId = invNew.Id;
                        InvoiceDetail newInvoiceDetail = new InvoiceDetail
                        {
                            InvoiceId = invNew.Id,
                            Amount = item.Amount,
                            AmountCrr = item.AmountCrr,
                            AmountVat = item.AmountVat,
                            CodingQuantity = item.CodingQuantity,
                            ConfirmationDate = item.ConfirmationDate,
                            CostId = item.CostId,
                            CreatedAt = DateTime.Today,
                            CreatedById = invNew.CreatedById,
                            DebetCredit = item.DebetCredit,
                            DeletedAt = item.DeletedAt,
                            Description = item.Description,
                            EPLDetailId = item.EPLDetailId,
                            IsConfirmed = item.IsConfirmed,
                            OfficeId = item.InvoiceId,
                            IsDeleted = item.IsDeleted,
                            PercentVat = item.PercentVat,
                            PerQty = item.PerQty,
                            Quantity = item.Quantity,
                            Sequence = item.Sequence,
                            Sign = item.Sign,
                            Type = item.Type,
                            UpdatedAt = item.UpdatedAt,
                            UpdatedById = item.UpdatedById
                        };
                        _invoiceDetailService.CreateObject(newInvoiceDetail, this);
                    }

                }
        }

        public void CreateInvoiceContraOnDeleteMode(Invoice existInvoice,IShipmentOrderService _shipmentOrderService,IInvoiceDetailService _invoiceDetailService,IVatService _vatService)
        {
            var shipment = _shipmentOrderService.GetObjectById(existInvoice.ShipmentOrderId);
                string shipmentNo = shipment.ShipmentOrderCode;

                int invNo = 0;
                string debetCredit = "";
                // As Debet
                if (existInvoice.DebetCredit == MasterConstant.DebetCredit.Debet)
                {
                    debetCredit = MasterConstant.DebetCredit.Credit;
                    // Get Last invNo as Contra
                    invNo = _repository.GetInvoiceNo(existInvoice.OfficeId,debetCredit) + 1;
                }
                // As Credit
                else if (existInvoice.DebetCredit == MasterConstant.DebetCredit.Credit)
                {
                    debetCredit = MasterConstant.DebetCredit.Debet;
                    // Get Last invNo as Contra
                    invNo = _repository.GetInvoiceNo(existInvoice.OfficeId, debetCredit) + 1;
                }

                // Create Contra Invoice based on Updated Invoice
                Invoice invNew = new Invoice();
                invNew.InvoicesNo = invNo;
                invNew.OfficeId = existInvoice.OfficeId;
                invNew.CreatedAt = DateTime.Now;
                invNew.ShipmentOrderId = existInvoice.ShipmentOrderId;
                invNew.CreatedById = existInvoice.CreatedById; 
                invNew.Printing = 0;
                invNew.ContactId = existInvoice.ContactId;
                invNew.CustomerName = existInvoice.CustomerName;
                invNew.CustomerAddress = existInvoice.CustomerAddress;
                invNew.BillId = existInvoice.BillId;
                invNew.BillName = existInvoice.BillName;
                invNew.BillAddress = existInvoice.BillAddress;
                invNew.JenisInvoices = existInvoice.JenisInvoices;
                invNew.PaymentUSD = existInvoice.PaymentUSD;
                invNew.PaymentIDR = existInvoice.PaymentIDR;
                invNew.DebetCredit = debetCredit;
                invNew.IsDeleted = true;
                invNew.DeletedAt = DateTime.Now;
                invNew.CustomerTypeId = existInvoice.CustomerTypeId;
                invNew.InvoicesEdit = "F";
                invNew.InvoicesTo = existInvoice.InvoicesTo;
                invNew.InvoicesAgent = existInvoice.InvoicesAgent;
                invNew.InvHeader = existInvoice.InvHeader;
                invNew.Rate = existInvoice.Rate;
                invNew.ExRateDate = existInvoice.ExRateDate;
                invNew.TotalVatIDR = existInvoice.TotalVatIDR;
                invNew.TotalVatUSD = existInvoice.TotalVatUSD;
                invNew.DueDate = existInvoice.DueDate;
                invNew.InvoiceStatus = existInvoice.InvoiceStatus + 1;
                invNew.LinkTo = "Cancel " + existInvoice.DebetCredit + "N No. " + existInvoice.InvoicesNo;
                
                invNew = _repository.CreateObject(invNew);

                // Create Contra Invoice Detail based on Updated Invoice
                List<InvoiceDetail> invDetailList = _invoiceDetailService.GetQueryable().Where(x => x.InvoiceId == existInvoice.Id).ToList();

                foreach (var item in invDetailList.ToList())
                {
                    InvoiceDetail newInvoiceDetail = new InvoiceDetail
                    {
                        InvoiceId = invNew.Id,
                        Amount = item.Amount,
                        AmountCrr = item.AmountCrr,
                        AmountVat = item.AmountVat,
                        CodingQuantity = item.CodingQuantity,
                        ConfirmationDate = item.ConfirmationDate,
                        CostId = item.CostId,
                        CreatedAt = DateTime.Today,
                        CreatedById = invNew.CreatedById,
                        DebetCredit = item.DebetCredit,
                        DeletedAt = item.DeletedAt,
                        Description = item.Description,
                        EPLDetailId = item.EPLDetailId,
                        IsConfirmed = item.IsConfirmed,
                        OfficeId = item.InvoiceId,
                        IsDeleted = item.IsDeleted,
                        PercentVat = item.PercentVat,
                        PerQty = item.PerQty,
                        Quantity = item.Quantity,
                        Sequence = item.Sequence,
                        Sign = item.Sign,
                        Type = item.Type,
                        UpdatedAt = item.UpdatedAt,
                        UpdatedById = item.UpdatedById
                    };
                    _invoiceDetailService.CreateObject(newInvoiceDetail, this);
                }
        }

        public Invoice CalculateTotalUSDIDR(int invoiceId, IInvoiceDetailService _invoiceDetailService)
        {
            IList<InvoiceDetail> eplD = _invoiceDetailService.GetQueryable().Where(x => x.InvoiceId == invoiceId && x.IsDeleted == false).ToList();
            decimal paymentIDR = 0;
            decimal paymentUSD = 0;
            decimal vatIDR = 0; 
            decimal vatUSD = 0;
            decimal amount = 0;
            foreach (var item in eplD)
            {
                amount = item.Amount.HasValue ? item.Amount.Value : 0;
                if (item.AmountCrr == MasterConstant.Currency.IDR)
                {
                    paymentIDR += amount * (item.Sign == true ? 1 : -1);
                    vatIDR += (item.AmountVat.HasValue ? item.AmountVat.Value : 0);
                }
                else
                {
                    paymentUSD += amount * (item.Sign == true ? 1 : -1);
                    vatUSD += (item.AmountVat.HasValue ? item.AmountVat.Value : 0);
                }

            }
            Invoice invoice = GetObjectById(invoiceId);
            invoice.PaymentIDR = paymentIDR;
            invoice.PaymentUSD = paymentUSD;
            invoice.TotalVatIDR = vatIDR;
            invoice.TotalVatUSD = vatUSD;
            invoice = _repository.UpdateObject(invoice);
            return invoice;
        }


        public Invoice CreateObject(Invoice invoice,IShipmentOrderService _shipmentOrderService,
            IContactService _contactService,IExchangeRateService _exchangeRateService)
        {
            invoice.Errors = new Dictionary<String, String>();
            if (isValid(_validator.VCreateObject(invoice,_shipmentOrderService)))
            {
                Invoice newInvoice  = new Invoice();
                newInvoice.IsDeleted = false;
                newInvoice.OfficeId = invoice.OfficeId;
                newInvoice.CreatedById = invoice.CreatedById;
                newInvoice.CreatedAt = DateTime.Today;
                newInvoice.InvoiceDate = DateTime.Today;
                newInvoice.ShipmentOrderId = invoice.ShipmentOrderId;
                switch (invoice.InvoicesTo)
                { 
                    case MasterConstant.InvoiceTo.InvoiceToShipper:
                        newInvoice.CustomerTypeId = MasterConstant.ContactType.Shipper; break;
                    case MasterConstant.InvoiceTo.InvoiceToConsignee:
                        newInvoice.CustomerTypeId = MasterConstant.ContactType.Consignee; break;
                    case MasterConstant.InvoiceTo.InvoiceToAgent:
                        newInvoice.CustomerTypeId = MasterConstant.ContactType.Agent; break;
                }

                newInvoice.ContactId = invoice.ContactId;
                //var customer = _contactService.GetObjectById(invoice.ContactId);
                //if (customer != null)
                //{
                //    var duedate = customer.CreditTermInDays;
                //    newInvoice.DueDate = duedate;
                //}
                newInvoice.CustomerName = !String.IsNullOrEmpty(invoice.CustomerName) ? invoice.CustomerName.ToUpper() : "";
                newInvoice.CustomerAddress = !String.IsNullOrEmpty(invoice.CustomerAddress) ? invoice.CustomerAddress.ToUpper() : "";
                newInvoice.DebetCredit = invoice.DebetCredit;
                newInvoice.InvoicesAgent = invoice.InvoicesAgent;
                newInvoice.InvoicesTo = invoice.InvoicesTo;
                newInvoice.InvoicesEdit = "F";
                newInvoice.InvHeader = invoice.InvHeader;
                newInvoice.JenisInvoices = invoice.JenisInvoices;
                newInvoice.CurrencyId = invoice.CurrencyId;
                newInvoice.BillId = invoice.BillId;
               // ExchangeRate LatestRate = _exchangeRateService.GetLatestRate(invoice.InvoiceDate);
               // newInvoice.Rate = LatestRate.ExRate1;
               // newInvoice.ExRateDate = LatestRate.ExRateDate;
               // newInvoice.ExRateId = LatestRate.Id;
                if (invoice.BillId > 0)
                {
                    newInvoice.BillName = !String.IsNullOrEmpty(invoice.BillName) ? invoice.BillName.ToUpper() : "";
                    newInvoice.BillAddress = !String.IsNullOrEmpty(invoice.BillAddress) ? invoice.BillAddress.ToUpper() : "";
                }
                newInvoice.ShipmentOrderId = invoice.ShipmentOrderId;
                newInvoice.InvoicesNo = _repository.GetInvoiceNo(invoice.OfficeId, invoice.DebetCredit) + 1;
                newInvoice.InvoiceStatus = _repository.GetNewInvoiceStatus(invoice.OfficeId, invoice.ShipmentOrderId) + 1;
                newInvoice = _repository.CreateObject(newInvoice);
                invoice.Id = newInvoice.Id;
            }
            return invoice;
        }

        public Invoice ConfirmObject(Invoice invoice,DateTime confirmationDate,IInvoiceDetailService _invoiceDetailService,IReceivableService _receiveableService)
        {
            if (isValid(_validator.VConfirmObject(invoice,_invoiceDetailService)))
            {
                IList<InvoiceDetail> invoiceDetails = _invoiceDetailService.GetQueryable().Where(x => x.InvoiceId == invoice.Id && x.IsDeleted == false).ToList();
                foreach (var invoiceDetail in invoiceDetails)
                {
                    invoiceDetail.Errors = new Dictionary<string, string>();
                    invoiceDetail.ConfirmationDate = confirmationDate;
                    _invoiceDetailService.ConfirmObject(invoiceDetail);
                    _receiveableService.CreateObject(invoice.OfficeId,invoice.ContactId, MasterConstant.SourceDocument.Invoice,invoice.Id, invoiceDetail.Id, invoiceDetail.AmountCrr.Value, (invoiceDetail.Amount ?? 0) + (invoiceDetail.AmountVat ?? 0), invoice.Rate ?? 0, invoice.InvoiceDate.AddDays(invoice.DueDate ?? 0));
                }
                invoice.ConfirmationDate = confirmationDate;
                invoice = _repository.ConfirmObject(invoice);
            }
            return invoice;
        }

        public Invoice Paid(Invoice invoice)
        {
                invoice.Paid = true;
                invoice.PaidOn = DateTime.Today;
                invoice = _repository.UpdateObject(invoice);
            return invoice;
        }

        public Invoice Unpaid(Invoice invoice)
        { 
            invoice.Paid = false;
            invoice.PaidOn = null;
            invoice = _repository.UpdateObject(invoice);
            return invoice;
        }

        public Invoice UnconfirmObject(Invoice invoice, DateTime confirmationDate, IInvoiceDetailService _invoiceDetailService, IReceivableService _receivableService, IReceiptVoucherDetailService _receiptVoucherDetailService)
        { 
            if (isValid(_validator.VUnConfirmObject(invoice,_receivableService,_receiptVoucherDetailService,_invoiceDetailService)))
            {
                IList<InvoiceDetail> invoiceDetails = _invoiceDetailService.GetQueryable().Where(x => x.InvoiceId == invoice.Id && x.IsDeleted == false).ToList();
                foreach (var invoiceDetail in invoiceDetails)
                {
                    invoiceDetail.Errors = new Dictionary<string, string>();
                    _invoiceDetailService.UnconfirmObject(invoiceDetail);
                    Receivable receivable = _receivableService.GetObjectBySource(MasterConstant.SourceDocument.Invoice, invoice.Id,invoiceDetail.Id);
                    _receivableService.SoftDeleteObject(receivable);
                }
                invoice = _repository.UnconfirmObject(invoice);
               
            }
            return invoice;
        }

        public Invoice UpdateObject(Invoice invoice,IShipmentOrderService _shipmentOrderService)
        {
            if (isValid(_validator.VUpdateObject(invoice, this,_shipmentOrderService)))
            {
                invoice.UpdatedAt = DateTime.Today;
                switch (invoice.InvoicesTo)
                {
                    case MasterConstant.InvoiceTo.InvoiceToShipper:
                        invoice.CustomerTypeId = MasterConstant.ContactType.Shipper; break;
                    case MasterConstant.InvoiceTo.InvoiceToConsignee:
                        invoice.CustomerTypeId = MasterConstant.ContactType.Consignee; break;
                    case MasterConstant.InvoiceTo.InvoiceToAgent:
                        invoice.CustomerTypeId = MasterConstant.ContactType.Agent; break;
                }
                invoice.CustomerName = !String.IsNullOrEmpty(invoice.CustomerName) ? invoice.CustomerName.ToUpper() : "";
                invoice.CustomerAddress = !String.IsNullOrEmpty(invoice.CustomerAddress) ? invoice.CustomerAddress.ToUpper() : "";
                if (invoice.BillId > 0)
                {
                    invoice.BillName = !String.IsNullOrEmpty(invoice.BillName) ? invoice.BillName.ToUpper() : "";
                    invoice.BillAddress = !String.IsNullOrEmpty(invoice.BillAddress) ? invoice.BillAddress.ToUpper() : "";
                }
                invoice = _repository.UpdateObject(invoice);
            }
            return invoice;
        }
          
        public Invoice SoftDeleteObject(Invoice invoice,IInvoiceDetailService _invoiceDetailService)
        {
            if (isValid(_validator.VSoftDeleteObject(invoice, this)))
            {
                invoice = _repository.SoftDeleteObject(invoice);
                //IList<InvoiceDetail> invoiceDetail = _invoiceDetailService.GetQueryable().
                //                                                           Where(x => x.InvoiceId == invoice.Id).ToList();
                //if (invoiceDetail != null)
                //{
                //    foreach (var item in invoiceDetail)
                //    {
                //        _invoiceDetailService.SoftDeleteObject(item, this, _invoiceDetailService);
                //    }
                //}
            }
            return invoice;
        }
          
        public Invoice Print(int Id,string fd )
        {
            Invoice invoice = GetObjectById(Id);
            if (isValid(_validator.VPrint(invoice)))
            {
                if (fd == MasterConstant.Print.Fixed)
                {
                    invoice.Printing = invoice.Printing == null ? 1 : (invoice.Printing.Value + 1);
                    if (invoice.PrintedAt == null)
                    {
                        invoice.PrintedAt = DateTime.Now;
                    }
                    _repository.UpdateObject(invoice);
                }
            }
            return invoice;
        }

        public Invoice CalculateTotalInvoice(Invoice invoice,IInvoiceDetailService _invoiceDetailService)
        {
                decimal totalIDR = 0;
                decimal totalVatIDR = 0;
                var invDetailList = _invoiceDetailService.GetQueryable().Where(x => x.InvoiceId == invoice.Id && x.OfficeId == invoice.OfficeId);
                foreach (var item in invDetailList)
                {
                        if (item.Sign == true)
                        {
                            totalIDR += item.Amount.HasValue ? item.Amount.Value : 0;
                            totalVatIDR += item.AmountVat.HasValue ? item.AmountVat.Value : 0;
                        }
                        else
                        {
                            totalIDR -= item.Amount.HasValue ? item.Amount.Value : 0;
                            totalVatIDR -= item.AmountVat.HasValue ? item.AmountVat.Value : 0;
                        }
                }
                invoice.PaymentUSD = totalIDR;
                invoice.TotalVatIDR = totalVatIDR;
                _repository.UpdateObject(invoice);
                return invoice;
        }

        public bool isValid(Invoice obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }
    }
}
