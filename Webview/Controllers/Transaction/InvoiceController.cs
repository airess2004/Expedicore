using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Service;
using Core.Interface.Service;
using Core.DomainModel;
using Data.Repository;
using Validation.Validation;
using System.Linq.Dynamic;
using System.Data.Entity;
using Core.Constant;
using Data.Context;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace WebView.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("InvoiceController");
        private IInvoiceService _invoiceService;
        private IExchangeRateService _exchangeRateService;
        private IInvoiceDetailService _invoiceDetailService;
        private IShipmentOrderService _shipmentOrderService;
        private IAccountUserService _accountUserService;
        private IContactService _contactService;
        private IOfficeService _officeService;
        public IVatService _vatService;
        private IReceivableService _receivableService;
        private IReceiptVoucherDetailService _receiptVoucherDetailService;

        public InvoiceController()
        {
            _invoiceService = new InvoiceService(new InvoiceRepository(), new InvoiceValidation());
            _invoiceDetailService = new InvoiceDetailService(new InvoiceDetailRepository(), new InvoiceDetailValidation());
            _shipmentOrderService = new ShipmentOrderService(new ShipmentOrderRepository(), new ShipmentOrderValidation());
            _officeService = new OfficeService(new OfficeRepository(),new OfficeValidation());
            _accountUserService = new AccountUserService(new AccountUserRepository(), new AccountUserValidator());
            _contactService = new ContactService(new ContactRepository(), new ContactValidation());
            _exchangeRateService = new ExchangeRateService(new ExchangeRateRepository(), new ExchangeRateValidation());
            _vatService = new VatService(new VatRepository(), new VatValidation());
            _receivableService = new ReceivableService(new ReceivableRepository(), new ReceivableValidator());
            _receiptVoucherDetailService = new ReceiptVoucherDetailService(new ReceiptVoucherDetailRepository(), new ReceiptVoucherDetailValidator());
        }


        public ActionResult Index()
        {
            //if (!AuthenticationModel.IsAllowed("View", Core.Constants.Constant.MenuName.Invoice, Core.Constants.Constant.MenuGroupName.Master))
            //{
            //    return Content(Core.Constants.Constant.ErrorPage.PageViewNotAllowed);
            //}

            return View();
        }

        public dynamic GetList(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "",
                                                int JobId = MasterConstant.JobType.PPJK)
        {
            // Construct where statement
            int officeid = _accountUserService.GetObjectById(AuthenticationModel.GetUserId()).OfficeId;

            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _invoiceService.GetQueryable().Where(x => x.OfficeId == officeid && x.ShipmentOrder.JobId == JobId); //.Include("Invoice").Include("UoM");

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.IsDeleted,
                             model.IsConfirmed,
                             JenisInvoice = model.JenisInvoices == "I" ? "Invoice" : "General",
                             model.InvoicesNo,
                             Paid = model.Paid == null ? false : model.Paid,
                             ShipmentOrderCode = model.ShipmentOrder.ShipmentOrderCode,
                             model.InvoiceStatus,
                             model.DebetCredit,
                             model.LinkTo,
                             Contact = model.Contact.ContactName,
                             model.PaymentUSD,
                             model.PaymentIDR,
                             model.DueDate,
                             model.PrintedAt,
                             model.Rate,
                             model.ExRateDate,
                             model.CreatedAt,
                             CreatedBy = model.CreatedBy.Name,
                             model.Printing,
                             model.DeletedAt,
                             model.PaidOn,
                             model.TotalVatUSD,
                             model.TotalVatIDR,
                             model.UpdatedAt,
                         }).Where(filter).OrderBy(sidx + " " + sord); //.ToList();

            var list = query.AsEnumerable();

            var pageIndex = Convert.ToInt32(page) - 1;
            var pageSize = rows;
            var totalRecords = query.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
            // default last page
            if (totalPages > 0)
            {
                if (!page.HasValue)
                {
                    pageIndex = totalPages - 1;
                    page = totalPages;
                }
            }

            list = list.Skip(pageIndex * pageSize).Take(pageSize);

            return Json(new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = (
                    from model in list
                    select new
                    {
                        id = model.Id,
                        cell = new object[] {
                             model.IsDeleted,
                             model.IsConfirmed,
                             model.JenisInvoice,
                             model.InvoicesNo,
                             model.Paid,
                             model.ShipmentOrderCode,
                             model.InvoiceStatus,
                             model.DebetCredit,
                             model.LinkTo,
                             model.Contact,
                             model.PaymentUSD,
                             model.PaymentIDR,
                             model.DueDate,
                             model.PrintedAt,
                             model.Rate,
                             model.ExRateDate,
                             model.CreatedAt,
                             model.CreatedBy,
                             model.Printing,
                             model.DeletedAt,
                             model.PaidOn,
                             model.TotalVatUSD,
                             model.TotalVatIDR,
                        }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        } 

        public dynamic GetLookUpRV(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "",
                                                int CustomerId = 0, int CurrencyId = 0)
        {
            // Construct where statement
            int officeid = _accountUserService.GetObjectById(AuthenticationModel.GetUserId()).OfficeId;
         
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _invoiceService.GetQueryable().Where(x => x.OfficeId == officeid && x.IsConfirmed == true &&
                (!x.Paid.HasValue || x.Paid.Value == false) && x.ContactId == CustomerId && x.CurrencyId == CurrencyId);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.InvoicesNo,
                             ShipmentOrderCode = model.ShipmentOrder.ShipmentOrderCode,
                             model.PaymentUSD,
                             model.PaymentIDR,
                             model.CreatedAt,
                             CreatedBy = model.CreatedBy.Name,
                             model.Printing,
                         }).Where(filter).OrderBy(sidx + " " + sord); //.ToList();

            var list = query.AsEnumerable();

            var pageIndex = Convert.ToInt32(page) - 1;
            var pageSize = rows;
            var totalRecords = query.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
            // default last page
            if (totalPages > 0)
            {
                if (!page.HasValue)
                {
                    pageIndex = totalPages - 1;
                    page = totalPages;
                }
            }

            list = list.Skip(pageIndex * pageSize).Take(pageSize);

            return Json(new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = (
                    from model in list
                    select new
                    {
                        id = model.Id,
                        cell = new object[] {
                             model.InvoicesNo,
                             model.ShipmentOrderCode,
                             model.PaymentUSD,
                             model.PaymentIDR,
                        }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }


        //Payable
        public dynamic GetLookUpRVDetail(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "",
                                              int PRId = 0)
        {
            // Construct where statement 
            int officeid = _accountUserService.GetObjectById(AuthenticationModel.GetUserId()).OfficeId;

            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            //var q = _payableService.GetQueryable().Where(x => x.PayableSource == MasterConstant.SourceDocument.PaymentRequest
            //    && x.PayableSourceId == PRId && x.RemainingAmount > 0 && x.IsDeleted == false);
            int PRNo = _invoiceService.GetObjectById(PRId).InvoicesNo;
            using (var db = new ExpedicoEntities())
            {
                var query = (from a in db.Receivables
                             where a.ReceivableSource == MasterConstant.SourceDocument.Invoice && a.ReceivableSourceId == PRId
                             && a.IsDeleted == false
                             select new
                             {
                                 Id = a.Id, 
                                 InvoiceNo = PRNo,
                                 AmountIDR = a.CurrencyId == MasterConstant.Currency.IDR ? a.Amount : 0,
                                 RemainingAmountIDR = a.CurrencyId == MasterConstant.Currency.IDR ? a.RemainingAmount : 0,
                                 AmountUSD = a.CurrencyId == MasterConstant.Currency.USD ? a.Amount : 0,
                                 RemainingAmountUSD = a.CurrencyId == MasterConstant.Currency.USD ? a.RemainingAmount : 0,
                                 Description = db.InvoiceDetails.Where(x => x.Id == a.ReceivableSourceDetailId).FirstOrDefault().Description,
                             }).AsQueryable().Where(filter).OrderBy(sidx + " " + sord);
                var list = query.AsEnumerable();

                var pageIndex = Convert.ToInt32(page) - 1;
                var pageSize = rows;
                var totalRecords = query.Count();
                var totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
                // default last page
                if (totalPages > 0)
                {
                    if (!page.HasValue)
                    {
                        pageIndex = totalPages - 1;
                        page = totalPages;
                    }
                }

                list = list.Skip(pageIndex * pageSize).Take(pageSize);

                return Json(new
                {
                    total = totalPages,
                    page = page,
                    records = totalRecords,
                    rows = (
                        from model in list
                        select new
                        {
                            id = model.Id,
                            cell = new object[] {
                             model.InvoiceNo,
                             model.Description,
                             model.AmountUSD,
                             model.RemainingAmountUSD,
                             model.AmountIDR,
                             model.RemainingAmountIDR
                        }
                        }).ToArray()
                }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Detail(int JobId)
        {
            // Access Level Validation
            //if (!AccountModels.IsAllowView(ConfigurationModels.MENU_FILE_SHIPMENT_ORDER, AccountModels.GetUserId()))
            //{
            //    // Response as Errors
            //    return Content("<b>You Don't Have Permision To Access This Page...!!!</b>");
            //}
            // END Access Level Validation

            VatController objJob = new VatController();
            try
            {
               // objJob.CurrencyList = (from c in _countryService.GetCurrencyList() where c != "" orderby c select c).Distinct().ToList();
              //  objJob.JobOwnerList = (from j in _jobownerService.GetJobOwner("", "", "") select j).Distinct().ToList();
              //  objJob.VesselTypeList = _invoiceService.GetVesselTypeList();
                
                switch (JobId)
                {
                    case MasterConstant.JobType.SeaExport:
                        ViewBag.Title = "Shipment Order - Sea Export";
                        return View("SeaExportDetail", objJob);
                    case MasterConstant.JobType.SeaImport:
                        ViewBag.Title = "Shipment Order - Sea Import";
                        return View("SeaImportDetail", objJob);
                    case MasterConstant.JobType.AirExport:
                        ViewBag.Title = "Shipment Order - Air Export";
                        return View("AirExportDetail", objJob);
                    case MasterConstant.JobType.AirImport:
                        ViewBag.Title = "Shipment Order - Air Import";
                        return View("AirImportDetail", objJob);
                    case MasterConstant.JobType.EMKLSeaExport:
                        ViewBag.Title = "Shipment Order - EMKL Sea Export";
                        return View("EMKLSeaExportDetail", objJob);
                    case MasterConstant.JobType.EMKLSeaImport:
                        ViewBag.Title = "Shipment Order - EMKL Sea Import";
                        return View("EMKLSeaImportDetail", objJob);
                    case MasterConstant.JobType.EMKLAirExport:
                        ViewBag.Title = "Shipment Order - EMKL Air Export";
                        return View("EMKLAirExportDetail", objJob);
                    case MasterConstant.JobType.EMKLAirImport:
                        ViewBag.Title = "Shipment Order - EMKL Air Import";
                        return View("EMKLAirImportDetail", objJob);
                    case MasterConstant.JobType.EMKLDomestic:
                        ViewBag.Title = "Shipment Order - EMKL Domestic";
                        return View("EMKLDomesticDetail", objJob);
                    case MasterConstant.JobType.PPJK:
                        ViewBag.Title = "Shipment Order - PPJK";
                        return View("PPJK", objJob);
                }

                LOG.Info("Invalid Shipment Order Detail page, UserName: " + AuthenticationModel.GetUserName() + ", CompanyId: " 
                    + ", jobId: " + JobId);

            }
            catch (Exception ex)
            {
                LOG.Error("Detail", ex);
            }
            return View();
        }

       
        public dynamic GetInfo(int Id)
        {
            Invoice model = new Invoice();
            try
            {
                model = _invoiceService.GetObjectById(Id);

                return Json(new
                {
                    model.Id,
                    model.ShipmentOrderId,
                    model.SaveOR,
                    model.BadDebt,
                    LinkTo = model.LinkTo == null ? "" : model.LinkTo,
                    model.ShipmentOrder.JobNumber,
                    model.ShipmentOrder.SubJobNumber,
                    model.ShipmentOrder.JobId,
                    model.OfficeId,
                    model.Office.InitialCompany,
                    model.InvoicesNo,
                    model.DebetCredit,
                    ETDETA = model.ShipmentOrder.ETD,
                    model.DueDate,
                    model.ShipmentOrder.ShipmentOrderCode,
                    model.PrintedAt,
                    model.Printing,
                    model.JenisInvoices,
                    model.ContactId,
                    CustomerCode = model.Contact.MasterCode,
                    model.CustomerName,
                    model.CustomerAddress,
                    Paid = model.Paid == null ? false : model.Paid.Value,
                    PaymentUSD = model.PaymentUSD != null ? model.PaymentUSD.Value : 0,
                    PaymentIDR = model.PaymentIDR != null ? model.PaymentIDR.Value : 0,
                    TotalVatUSD = model.TotalVatUSD != null ? model.TotalVatUSD.Value : 0,
                    TotalVatIDR = model.TotalVatIDR != null ? model.TotalVatIDR.Value : 0,
                    model.CurrencyId,
                    model.Rate,
                    model.ExRateDate,
                    ListInvoiceDetail = (from detail in model.InvoiceDetails
                                  where detail.IsDeleted == false 
                                  select new
                                  {
                                      detail.Id,
                                      detail.EPLDetailId,
                                      AccountCode = detail.Cost.MasterCode,
                                      AccountName = detail.Cost.Name,
                                      detail.Description,
                                      detail.Type,
                                      detail.AmountCrr,
                                      detail.Amount,
                                      detail.Quantity,
                                      detail.PerQty,
                                      detail.CodingQuantity,
                                      detail.Sign,
                                      detail.VatId,
                                      detail.AmountVat,
                                      detail.PercentVat
                                  }).ToArray(),
                    model.Errors
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LOG.Error("GetInfo", ex);
                Dictionary<string, string> Errors = new Dictionary<string, string>();
                Errors.Add("Generic", "Error " + ex);

                return Json(new
                {
                    Errors
                }, JsonRequestBehavior.AllowGet);
            }


        }

        [HttpPost]
        public dynamic Insert(Invoice model)
        {
            ////try
            {
                //if (!AuthenticationModel.IsAllowed("Create", Core.Constants.Constant.MenuName.Invoice, Core.Constants.Constant.MenuGroupName.Master))
                //{
                //    Dictionary<string, string> Errors = new Dictionary<string, string>();
                //    Errors.Add("Generic", "You are Not Allowed to Add record");

                //    return Json(new
                //    {
                //        Errors
                //    }, JsonRequestBehavior.AllowGet);
                //}
                int userId = AuthenticationModel.GetUserId();
                model.CreatedById = userId;
                model.OfficeId = _accountUserService.GetObjectById(userId).OfficeId;
                model = _invoiceService.CreateObject(model,_shipmentOrderService,_contactService,_exchangeRateService);
                
                return Json(new
                {
                    model.Errors,
                    invoiceId = model.Id,
                });
            }
            //catch (Exception ex)
            //{
            //    LOG.Error("Insert Failed", ex);
            //    Dictionary<string, string> Errors = new Dictionary<string, string>();
            //    model.Errors.Add("Generic", "Insert Failed " + ex);

            //    return Json(new
            //    {
            //        model.Errors,
            //    });
            //}


        }

        [HttpPost] 
        public dynamic InsertDetail(InvoiceDetail model)
        {
            ////try
            {
                //if (!AuthenticationModel.IsAllowed("Create", Core.Constants.Constant.MenuName.Invoice, Core.Constants.Constant.MenuGroupName.Master))
                //{
                //    Dictionary<string, string> Errors = new Dictionary<string, string>();
                //    Errors.Add("Generic", "You are Not Allowed to Add record");

                //    return Json(new
                //    {
                //        Errors
                //    }, JsonRequestBehavior.AllowGet);
                //}
                int userId = AuthenticationModel.GetUserId();
                model.CreatedById = userId;
                model.OfficeId = _accountUserService.GetObjectById(userId).OfficeId;
                model = _invoiceDetailService.CreateObject(model, _invoiceService);

                return Json(new
                {
                    model.Id,
                    model.Errors,
                });
            }
            //catch (Exception ex)
            //{
            //    LOG.Error("Insert Failed", ex);
            //    Dictionary<string, string> Errors = new Dictionary<string, string>();
            //    model.Errors.Add("Generic", "Insert Failed " + ex);

            //    return Json(new
            //    {
            //        model.Errors,
            //    });
            //}


        }

        [HttpPost]
        public dynamic Update(Invoice model)
        {
            try
            {
                //if (!AuthenticationModel.IsAllowed("Edit", Core.Constants.Constant.MenuName.Invoice, Core.Constants.Constant.MenuGroupName.Master))
                //{
                //    Dictionary<string, string> Errors = new Dictionary<string, string>();
                //    Errors.Add("Generic", "You are Not Allowed to Edit record");

                //    return Json(new
                //    {
                //        Errors
                //    }, JsonRequestBehavior.AllowGet);
                //}

                int userId = AuthenticationModel.GetUserId();
                model.CreatedById = userId;
                model.OfficeId = _accountUserService.GetObjectById(userId).OfficeId;
                var data = _invoiceService.GetObjectById(model.Id);
                model = _invoiceService.UpdateObject(data,_shipmentOrderService);
            }
            catch (Exception ex)
            {
                LOG.Error("Update Failed", ex);
                Dictionary<string, string> Errors = new Dictionary<string, string>();
                Errors.Add("Generic", "Update Failed " + ex);

                return Json(new
                {
                    Errors
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                model.Errors
            });
        }
         
        [HttpPost]
        public dynamic UpdateDetail(InvoiceDetail model)
        {
            try
            {
                //if (!AuthenticationModel.IsAllowed("Edit", Core.Constants.Constant.MenuName.Invoice, Core.Constants.Constant.MenuGroupName.Master))
                //{
                //    Dictionary<string, string> Errors = new Dictionary<string, string>();
                //    Errors.Add("Generic", "You are Not Allowed to Edit record");

                //    return Json(new
                //    {
                //        Errors
                //    }, JsonRequestBehavior.AllowGet);
                //}

                int userId = AuthenticationModel.GetUserId();
                var data = _invoiceDetailService.GetObjectById(model.Id);
                data.UpdatedById = userId;
                data.OfficeId = _accountUserService.GetObjectById(userId).OfficeId;
                data.EPLDetailId = model.EPLDetailId;
                data.CostId = model.CostId;
                data.Description = model.Description;
                data.Quantity = model.Quantity;
                data.PerQty = model.PerQty;
                data.Sign = model.Sign;
                data.Type = model.Type;
                data.CodingQuantity = model.CodingQuantity;
                data.AmountCrr = model.AmountCrr;
                data.Amount = model.Amount;
                data.AmountVat = model.AmountVat;
                data.PercentVat = model.PercentVat;
                data.VatId = model.VatId;
                model = _invoiceDetailService.UpdateObject(data,_invoiceService,_invoiceDetailService);
            }
            catch (Exception ex)
            {
                LOG.Error("Update Failed", ex);
                Dictionary<string, string> Errors = new Dictionary<string, string>();
                Errors.Add("Generic", "Update Failed " + ex);

                return Json(new
                {
                    Errors
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                model.Id,
                model.Errors
            });
        }


        [HttpPost]
        public dynamic Delete(Invoice model)
        {
            try
            {
                //if (!AuthenticationModel.IsAllowed("Delete", Core.Constants.Constant.MenuName.Invoice, Core.Constants.Constant.MenuGroupName.Master))
                //{
                //    Dictionary<string, string> Errors = new Dictionary<string, string>();
                //    Errors.Add("Generic", "You are Not Allowed to Delete Record");

                //    return Json(new
                //    {
                //        Errors
                //    }, JsonRequestBehavior.AllowGet);
                //}

                var data = _invoiceService.GetObjectById(model.Id);
                int userId = AuthenticationModel.GetUserId();
                data.OfficeId = _accountUserService.GetObjectById(userId).OfficeId;
                model = _invoiceService.SoftDeleteObject(data,_invoiceDetailService);
            }
            catch (Exception ex)
            {
                LOG.Error("Delete Failed", ex);
                Dictionary<string, string> Errors = new Dictionary<string, string>();
                Errors.Add("Generic", "Delete Failed " + ex);

                return Json(new
                {
                    Errors
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                model.Errors
            });

        }

        [HttpPost]
        public dynamic DeleteDetail(InvoiceDetail model)
        { 
            try
            {
                //if (!AuthenticationModel.IsAllowed("Delete", Core.Constants.Constant.MenuName.Invoice, Core.Constants.Constant.MenuGroupName.Master))
                //{
                //    Dictionary<string, string> Errors = new Dictionary<string, string>();
                //    Errors.Add("Generic", "You are Not Allowed to Delete Record");

                //    return Json(new
                //    {
                //        Errors
                //    }, JsonRequestBehavior.AllowGet);
                //}

                var data = _invoiceDetailService.GetObjectById(model.Id);
                int userId = AuthenticationModel.GetUserId();
                data.CreatedById = userId;
                data.OfficeId = _accountUserService.GetObjectById(userId).OfficeId;
                model = _invoiceDetailService.SoftDeleteObject(data,_invoiceService,_invoiceDetailService);
            }
            catch (Exception ex)
            {
                LOG.Error("Delete Failed", ex);
                Dictionary<string, string> Errors = new Dictionary<string, string>();
                Errors.Add("Generic", "Delete Failed " + ex);

                return Json(new
                {
                    Errors
                }, JsonRequestBehavior.AllowGet);
            }
             
            return Json(new
            {
                model.Errors
            });

        }

        [HttpPost]
        public dynamic Confirm(int id)
        {
            Invoice model = new Invoice();
            try
            {
                //if (!AuthenticationModel.IsAllowed("Confirm", Core.Constants.Constant.MenuName.StockAdjustment, Core.Constants.Constant.MenuGroupName.Master))
                //{
                //    Dictionary<string, string> Errors = new Dictionary<string, string>();
                //    Errors.Add("Generic", "You are Not Allowed to Confirm Record");

                //    return Json(new
                //    {
                //        Errors
                //    }, JsonRequestBehavior.AllowGet);
                //} 
                model = _invoiceService.GetObjectById(id);
                int userId = AuthenticationModel.GetUserId();
                DateTime ConfirmationDate = DateTime.Now;
                model = _invoiceService.ConfirmObject(model, ConfirmationDate, _invoiceDetailService, _receivableService);
            }
            catch (Exception ex)
            {
                LOG.Error("Confirm Failed", ex);
                Dictionary<string, string> Errors = new Dictionary<string, string>();
                Errors.Add("Generic", "Error " + ex);

                return Json(new
                {
                  Errors
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                model.Errors
            });
        }

        [HttpPost]
        public dynamic Unconfirm(int id)
        {
            Invoice model = new Invoice();
            try
            {
                //if (!AuthenticationModel.IsAllowed("Confirm", Core.Constants.Constant.MenuName.StockAdjustment, Core.Constants.Constant.MenuGroupName.Master))
                //{
                //    Dictionary<string, string> Errors = new Dictionary<string, string>();
                //    Errors.Add("Generic", "You are Not Allowed to Confirm Record");

                //    return Json(new
                //    {
                //        Errors
                //    }, JsonRequestBehavior.AllowGet);
                //} 
                model = _invoiceService.GetObjectById(id);
                int userId = AuthenticationModel.GetUserId();
                DateTime ConfirmationDate = DateTime.Now;
                model = _invoiceService.UnconfirmObject(model, ConfirmationDate, _invoiceDetailService, _receivableService, _receiptVoucherDetailService);
            }
            catch (Exception ex)
            {
                LOG.Error("Confirm Failed", ex);
                Dictionary<string, string> Errors = new Dictionary<string, string>();
                Errors.Add("Generic", "Error " + ex);

                return Json(new
                {
                    Errors
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                model.Errors
            });
        }

        public ActionResult Print(int Id, string fd)
        { 
            ReportDocument rd = new ReportDocument();
            try
            {
                //// Access Level Validation
                //if (!AccountModels.IsAllowPrint(ConfigurationModels.MENU_FILE_INVOICE, AccountModels.GetUserId()))
                //{
                //    // Response as Errors
                //    return Content("<b>You Don't Have Permision To Access This Page...!!!</b>");
                //}
                //// END Access Level Validation
                int userId = AuthenticationModel.GetUserId();
                int OfficeId = _accountUserService.GetObjectById(userId).OfficeId;
                fd = String.IsNullOrEmpty(fd) ? "" : fd;
                Invoice model = _invoiceService.Print(Id, fd);
                
                if (model.Errors.Any())
                {
                    return Content(model.Errors.FirstOrDefault().Value.ToString());
                }
                else
                {
                    var query = _invoiceDetailService.GetQueryable().Where(x=> x.InvoiceId == Id && x.IsDeleted == false);
                    var listdata = (from data in query
                                    select new
                                    {
                                        ContactName = data.Invoices.Contact.ContactName,
                                        ContactAddress = data.Invoices.Contact.ContactAddress,
                                        PrintDate = data.Invoices.PrintedAt.Value,
                                        InvoiceNo = data.Invoices.InvoicesNo,
                                        ShipmentOrderNo = data.Invoices.ShipmentOrder.ShipmentOrderCode,
                                        PONo = data.Invoices.ShipmentOrder.JobOrderCustomer,
                                        BLNo = data.Invoices.ShipmentOrder.HouseBLNo,
                                        ContainerQTY = data.Invoices.ShipmentOrder.SeaContainers.Count,
                                        Description = data.Description,
                                        Amount = data.Amount ?? 0,
                                        TaxAmount = data.AmountVat ?? 0
                                    }).ToList();
                    rd.Load(Server.MapPath("~/") + "Reports/Invoice.rpt");

                    // Setting report data source
                    rd.SetDataSource(listdata);

                    var stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    return File(stream, "application/pdf");
                }
            }
            catch (Exception ex)
            {
               LOG.Error("Confirm Failed", ex);
                Dictionary<string, string> Errors = new Dictionary<string, string>();
                Errors.Add("Generic", "Error " + ex);
                return Content(Errors.FirstOrDefault().Value.ToString());
            }
        }
       
    }
}
