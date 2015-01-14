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
using CrystalDecisions.CrystalReports.Engine;

namespace WebView.Controllers
{
    public class ReceiptVoucherController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("ReceiptVoucherController");
        private IReceiptVoucherService _receiptVoucherService;
        private IExchangeRateService _exchangeRateService;
        private IReceiptVoucherDetailService _receiptVoucherDetailService;
        private IShipmentOrderService _shipmentOrderService;
        private IAccountUserService _accountUserService;
        private IContactService _contactService;
        private IOfficeService _officeService;
        public IVatService _vatService;
        private IReceivableService _receivableService;
        private ICashBankService _cashBankService;
        private IInvoiceService _invoiceService;
        private ICashMutationService _cashMutationService;

        public ReceiptVoucherController()
        {
            _receiptVoucherService = new ReceiptVoucherService(new ReceiptVoucherRepository(), new ReceiptVoucherValidator());
            _shipmentOrderService = new ShipmentOrderService(new ShipmentOrderRepository(), new ShipmentOrderValidation());
            _officeService = new OfficeService(new OfficeRepository(), new OfficeValidation());
            _accountUserService = new AccountUserService(new AccountUserRepository(), new AccountUserValidator());
            _contactService = new ContactService(new ContactRepository(), new ContactValidation());
            _exchangeRateService = new ExchangeRateService(new ExchangeRateRepository(), new ExchangeRateValidation());
            _vatService = new VatService(new VatRepository(), new VatValidation());
            _receivableService = new ReceivableService(new ReceivableRepository(), new ReceivableValidator());
            _receiptVoucherDetailService = new ReceiptVoucherDetailService(new ReceiptVoucherDetailRepository(), new ReceiptVoucherDetailValidator());
            _cashBankService = new CashBankService(new CashBankRepository(), new CashBankValidator());
            _invoiceService = new InvoiceService(new InvoiceRepository(), new InvoiceValidation());
            _cashMutationService = new CashMutationService(new CashMutationRepository(), new CashMutationValidator());
        }


        public ActionResult Index()
        {
            //if (!AuthenticationModel.IsAllowed("View", Core.Constants.Constant.MenuName.ReceiptVoucher, Core.Constants.Constant.MenuGroupName.Master))
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
            var q = _receiptVoucherService.GetQueryable().Where(x => x.OfficeId == officeid);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.IsDeleted,
                             model.IsConfirmed,
                             model.Code,
                             CashBankName = model.CashBank.Name,
                             model.TotalAmountIDR,
                             model.TotalAmountUSD,
                             ContactName = model.Contact.ContactName,
                             model.CreatedAt,
                             CreatedBy = model.CreatedBy.Name,
                             model.UpdatedAt,
                         }).Where(filter).OrderBy(sidx + " " + sord);

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
                             model.Code,
                             model.CashBankName,
                             model.TotalAmountIDR,
                             model.TotalAmountUSD, 
                             model.ContactName,
                             model.CreatedAt,
                             model.CreatedBy,
                             model.UpdatedAt,
                        }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Detail()
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
                LOG.Info("Invalid Shipment Order Detail page, UserName: " + AuthenticationModel.GetUserName() + ", CompanyId: ");
            }
            catch (Exception ex)
            {
                LOG.Error("Detail", ex);
            }
            return View();
        }


        public dynamic GetInfo(int Id)
        {
            ReceiptVoucher model = new ReceiptVoucher();
            try
            {
                model = _receiptVoucherService.GetObjectById(Id);

                return Json(new
                {
                    model.Id,
                    model.Code,
                    CashBankCode = model.CashBank.MasterCode,
                    CashBankName = model.CashBank.Name,
                    model.CashBankId,
                    CurrencyId = model.CashBank.CurrencyId,
                    Currency = model.CashBank.CurrencyId == MasterConstant.Currency.IDR ? "IDR" : "USD",
                    model.Rate,
                    model.ExRateDate,
                    ContactAddress = model.Contact.ContactAddress,
                    ContactCode = model.Contact.MasterCode,
                    model.ContactId,
                    ContactName = model.Contact.ContactName,
                    ListReceiptVoucherDetail = (from detail in model.ReceiptVoucherDetails
                                                where detail.IsDeleted == false
                                                select new
                                                {
                                                    detail.Id,
                                                    detail.Description,
                                                    detail.AmountIDR, 
                                                    detail.AmountUSD,
                                                    ReceivableCode = detail.Receivable.Code,
                                                    ShipmentOrderNo = _invoiceService.GetObjectById(detail.Receivable.ReceivableSourceId).ShipmentOrder.ShipmentOrderCode,
                                                    RemainingAmountIDR = detail.Receivable.CurrencyId == MasterConstant.Currency.IDR ? detail.Receivable.RemainingAmount : 0,
                                                    RemainingAmountUSD = detail.Receivable.CurrencyId == MasterConstant.Currency.USD ? detail.Receivable.RemainingAmount : 0,
                                                    detail.ReceivableId,

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
        public dynamic Insert(ReceiptVoucher model)
        {
            ////try
            {
                //if (!AuthenticationModel.IsAllowed("Create", Core.Constants.Constant.MenuName.ReceiptVoucher, Core.Constants.Constant.MenuGroupName.Master))
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
                model = _receiptVoucherService.CreateObject(model, _receiptVoucherDetailService, _receivableService, _contactService, _cashBankService);

                return Json(new
                {
                    model.Errors,
                    receiptVoucherId = model.Id,
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
        public dynamic InsertDetail(ReceiptVoucherDetail model)
        {
            ////try
            {
                //if (!AuthenticationModel.IsAllowed("Create", Core.Constants.Constant.MenuName.ReceiptVoucher, Core.Constants.Constant.MenuGroupName.Master))
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
                model = _receiptVoucherDetailService.CreateObject(model, _receiptVoucherService, _cashBankService, _receivableService);

                return Json(new
                {
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
        public dynamic Update(ReceiptVoucher model)
        {
            try
            {
                //if (!AuthenticationModel.IsAllowed("Edit", Core.Constants.Constant.MenuName.ReceiptVoucher, Core.Constants.Constant.MenuGroupName.Master))
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
                var data = _receiptVoucherService.GetObjectById(model.Id);
                // model = _receiptVoucherService.UpdateObject(data,_);
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
        public dynamic UpdateDetail(ReceiptVoucherDetail model)
        {
            try
            {
                //if (!AuthenticationModel.IsAllowed("Edit", Core.Constants.Constant.MenuName.ReceiptVoucher, Core.Constants.Constant.MenuGroupName.Master))
                //{
                //    Dictionary<string, string> Errors = new Dictionary<string, string>();
                //    Errors.Add("Generic", "You are Not Allowed to Edit record");

                //    return Json(new
                //    {
                //        Errors
                //    }, JsonRequestBehavior.AllowGet);
                //}

                int userId = AuthenticationModel.GetUserId();
                var data = _receiptVoucherDetailService.GetObjectById(model.Id);
                data.ReceivableId = model.ReceivableId;
                data.AmountIDR = model.AmountIDR;
                data.AmountUSD = model.AmountUSD;
                data.Description = model.Description;
                data.ReceiptVoucherId = model.ReceiptVoucherId;
                data.UpdatedById = userId;
                model = _receiptVoucherDetailService.UpdateObject(data, _receiptVoucherService, _cashBankService, _receivableService);
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
        public dynamic Delete(ReceiptVoucher model)
        {
            try
            {
                //if (!AuthenticationModel.IsAllowed("Delete", Core.Constants.Constant.MenuName.ReceiptVoucher, Core.Constants.Constant.MenuGroupName.Master))
                //{
                //    Dictionary<string, string> Errors = new Dictionary<string, string>();
                //    Errors.Add("Generic", "You are Not Allowed to Delete Record");

                //    return Json(new
                //    {
                //        Errors
                //    }, JsonRequestBehavior.AllowGet);
                //}

                var data = _receiptVoucherService.GetObjectById(model.Id);
                int userId = AuthenticationModel.GetUserId();
                data.OfficeId = _accountUserService.GetObjectById(userId).OfficeId;
                model = _receiptVoucherService.SoftDeleteObject(data, _receiptVoucherDetailService);
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
        public dynamic DeleteDetail(ReceiptVoucherDetail model)
        {
            try
            {
                //if (!AuthenticationModel.IsAllowed("Delete", Core.Constants.Constant.MenuName.ReceiptVoucher, Core.Constants.Constant.MenuGroupName.Master))
                //{
                //    Dictionary<string, string> Errors = new Dictionary<string, string>();
                //    Errors.Add("Generic", "You are Not Allowed to Delete Record");

                //    return Json(new
                //    {
                //        Errors
                //    }, JsonRequestBehavior.AllowGet);
                //}

                var data = _receiptVoucherDetailService.GetObjectById(model.Id);
                int userId = AuthenticationModel.GetUserId();
                data.CreatedById = userId;
                data.OfficeId = _accountUserService.GetObjectById(userId).OfficeId;
                model = _receiptVoucherDetailService.SoftDeleteObject(data, _receiptVoucherService);
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
            ReceiptVoucher model = new ReceiptVoucher();
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
                model = _receiptVoucherService.GetObjectById(id);
                int userId = AuthenticationModel.GetUserId();
                DateTime ConfirmationDate = DateTime.Now;
                model = _receiptVoucherService.ConfirmObject(model, ConfirmationDate, _receiptVoucherDetailService, 
                    _cashBankService, _receivableService, _cashMutationService,_invoiceService);
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
            ReceiptVoucher model = new ReceiptVoucher();
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
                model = _receiptVoucherService.GetObjectById(id);
                int userId = AuthenticationModel.GetUserId();
                DateTime ConfirmationDate = DateTime.Now;
                model = _receiptVoucherService.UnconfirmObject(model, _receiptVoucherDetailService, 
                    _cashBankService,_receivableService, _cashMutationService,_invoiceService);
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

        public ActionResult Print(int Id)
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
                //Invoice model =   .Print(Id, fd);

                //if (model.Errors.Any())
                //{
                //    return Content(model.Errors.FirstOrDefault().Value.ToString());
                //}
                //else
                //{
                var query = _receiptVoucherDetailService.GetQueryable().Where(x => x.ReceiptVoucherId == Id && x.IsDeleted == false).ToList();
                var listdata = (from data in query
                                select new
                                {
                                    ContactName = data.ReceiptVoucher.Contact.ContactName,
                                    ContactAddress = data.ReceiptVoucher.Contact.ContactAddress,
                                    PrintDate = DateTime.Today.Date,
                                    PRNo = data.ReceiptVoucher.Code,
                                    ShipmentOrderNo = _invoiceService.GetObjectById(data.Receivable.ReceivableSourceId).ShipmentOrder.ShipmentOrderCode,
                                    Description = data.Description,
                                    Amount = data.AmountIDR + data.AmountUSD,
                                }).ToList();
                rd.Load(Server.MapPath("~/") + "Reports/ReceiptVoucher.rpt");

                // Setting report data source
                rd.SetDataSource(listdata);

                var stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                return File(stream, "application/pdf");
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
