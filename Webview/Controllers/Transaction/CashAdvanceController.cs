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
    public class CashAdvanceController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("CashAdvanceController");
        private ICashAdvanceService _cashAdvanceService;
        private IExchangeRateService _exchangeRateService;
        private ICashAdvanceDetailService _cashAdvanceDetailService;
        private IShipmentOrderService _shipmentOrderService;
        private IAccountUserService _accountUserService;
        private IContactService _contactService;
        private IOfficeService _officeService;
        public IVatService _vatService;
        private IPayableService _payableService;
        private ICashBankService _cashBankService;
        private IPaymentRequestService _paymentRequestService;
        private ICashMutationService _cashMutationService;

        public CashAdvanceController()
        {
            _cashAdvanceService = new CashAdvanceService(new CashAdvanceRepository(), new CashAdvanceValidation());
            _shipmentOrderService = new ShipmentOrderService(new ShipmentOrderRepository(), new ShipmentOrderValidation());
            _officeService = new OfficeService(new OfficeRepository(),new OfficeValidation());
            _accountUserService = new AccountUserService(new AccountUserRepository(), new AccountUserValidator());
            _contactService = new ContactService(new ContactRepository(), new ContactValidation());
            _exchangeRateService = new ExchangeRateService(new ExchangeRateRepository(), new ExchangeRateValidation());
            _vatService = new VatService(new VatRepository(), new VatValidation());
            _payableService = new PayableService(new PayableRepository(), new PayableValidator());
            _cashAdvanceDetailService = new CashAdvanceDetailService(new CashAdvanceDetailRepository(), new CashAdvanceDetailValidation());
            _cashBankService = new CashBankService(new CashBankRepository(), new CashBankValidator());
            _paymentRequestService = new PaymentRequestService(new PaymentRequestRepository(), new PaymentRequestValidation());
            _cashMutationService = new CashMutationService(new CashMutationRepository(), new CashMutationValidator());
        }


        public ActionResult Index()
        {
            //if (!AuthenticationModel.IsAllowed("View", Core.Constants.Constant.MenuName.CashAdvance, Core.Constants.Constant.MenuGroupName.Master))
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
            var q = _cashAdvanceService.GetQueryable().Where(x => x.OfficeId == officeid); 

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.IsDeleted,
                             model.IsConfirmed,
                             model.CashAdvanceNo,
                             model.Paid,
                             EmployeeName = model.Employee.Name,
                             model.CashAdvanceIDR,
                             model.CashAdvanceUSD,
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
                             model.CashAdvanceNo,
                             model.EmployeeName,
                             model.CashAdvanceIDR,
                             model.CashAdvanceUSD,
                        }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetLookUp(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "",
                                                int EmployeeId = 0)
        {
            // Construct where statement
            int officeid = _accountUserService.GetObjectById(AuthenticationModel.GetUserId()).OfficeId;

            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _cashAdvanceService.GetQueryable().Where(x => x.OfficeId == officeid && x.IsDeleted == false 
                && x.EmployeeId == EmployeeId && x.IsConfirmed == true);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.CashAdvanceNo,
                             model.CashAdvanceIDR,
                             model.CashAdvanceUSD,
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
                             model.CashAdvanceNo,
                             model.CashAdvanceUSD,
                             model.CashAdvanceIDR,
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
            CashAdvance model = new CashAdvance();
            try
            {
                model = _cashAdvanceService.GetObjectById(Id);

                return Json(new
                {
                    model.Id,
                    model.CashAdvanceNo,
                    model.CashAdvanceIDR,
                    model.CashAdvanceUSD,
                    model.EmployeeId,
                    EmployeeName = model.Employee.Name,
                    ListCashAdvanceDetail = (from detail in model.CashAdvanceDetails
                                                where detail.IsDeleted == false
                                                select new
                                                {
                                                    detail.Id,
                                                    detail.Description,
                                                    detail.AmountIDR,
                                                    detail.AmountUSD,
                                                    PayableCode = detail.Payable.Code,
                                                    ShipmentOrderNo = _paymentRequestService.GetObjectById(detail.Payable.PayableSourceId).ShipmentOrder.ShipmentOrderCode,
                                                    RemainingAmountIDR = detail.Payable.CurrencyId == MasterConstant.Currency.IDR ? detail.Payable.RemainingAmount : 0,
                                                    RemainingAmountUSD = detail.Payable.CurrencyId == MasterConstant.Currency.USD ? detail.Payable.RemainingAmount : 0,
                                                    detail.PayableId,
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
        public dynamic Insert(CashAdvance model)
        {
            ////try
            {
                //if (!AuthenticationModel.IsAllowed("Create", Core.Constants.Constant.MenuName.CashAdvance, Core.Constants.Constant.MenuGroupName.Master))
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
                model = _cashAdvanceService.CreateObject(model,_shipmentOrderService,_contactService);
                
                return Json(new
                {
                    model.Errors,
                    cashAdvanceId = model.Id,
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
        public dynamic InsertDetail(CashAdvanceDetail model)
        {
            ////try
            {
                //if (!AuthenticationModel.IsAllowed("Create", Core.Constants.Constant.MenuName.CashAdvance, Core.Constants.Constant.MenuGroupName.Master))
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
                model = _cashAdvanceDetailService.CreateObject(model, _shipmentOrderService, _cashAdvanceService);

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
        public dynamic Update(CashAdvance model)
        {
            try
            {
                //if (!AuthenticationModel.IsAllowed("Edit", Core.Constants.Constant.MenuName.CashAdvance, Core.Constants.Constant.MenuGroupName.Master))
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
                var data = _cashAdvanceService.GetObjectById(model.Id);
                model = _cashAdvanceService.UpdateObject(data);
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
        public dynamic UpdateDetail(CashAdvanceDetail model)
        {
            try
            {
                //if (!AuthenticationModel.IsAllowed("Edit", Core.Constants.Constant.MenuName.CashAdvance, Core.Constants.Constant.MenuGroupName.Master))
                //{
                //    Dictionary<string, string> Errors = new Dictionary<string, string>();
                //    Errors.Add("Generic", "You are Not Allowed to Edit record");

                //    return Json(new
                //    {
                //        Errors
                //    }, JsonRequestBehavior.AllowGet);
                //}

                int userId = AuthenticationModel.GetUserId();
                var data = _cashAdvanceDetailService.GetObjectById(model.Id);
                data.AmountIDR = model.AmountIDR;
                data.AmountUSD = model.AmountUSD;
                data.Description = model.Description;
                data.CashAdvanceId = model.CashAdvanceId;
                data.UpdatedById = userId;
                model = _cashAdvanceDetailService.UpdateObject(data,_cashAdvanceService);
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
        public dynamic Delete(CashAdvance model)
        {
            try
            {
                //if (!AuthenticationModel.IsAllowed("Delete", Core.Constants.Constant.MenuName.CashAdvance, Core.Constants.Constant.MenuGroupName.Master))
                //{
                //    Dictionary<string, string> Errors = new Dictionary<string, string>();
                //    Errors.Add("Generic", "You are Not Allowed to Delete Record");

                //    return Json(new
                //    {
                //        Errors
                //    }, JsonRequestBehavior.AllowGet);
                //}

                var data = _cashAdvanceService.GetObjectById(model.Id);
                int userId = AuthenticationModel.GetUserId();
                data.OfficeId = _accountUserService.GetObjectById(userId).OfficeId;
                model = _cashAdvanceService.SoftDeleteObject(data,_cashAdvanceDetailService);
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
        public dynamic DeleteDetail(CashAdvanceDetail model)
        { 
            try
            {
                //if (!AuthenticationModel.IsAllowed("Delete", Core.Constants.Constant.MenuName.CashAdvance, Core.Constants.Constant.MenuGroupName.Master))
                //{
                //    Dictionary<string, string> Errors = new Dictionary<string, string>();
                //    Errors.Add("Generic", "You are Not Allowed to Delete Record");

                //    return Json(new
                //    {
                //        Errors
                //    }, JsonRequestBehavior.AllowGet);
                //}

                var data = _cashAdvanceDetailService.GetObjectById(model.Id);
                int userId = AuthenticationModel.GetUserId();
                data.CreatedById = userId;
                data.OfficeId = _accountUserService.GetObjectById(userId).OfficeId;
                model = _cashAdvanceDetailService.SoftDeleteObject(data,_cashAdvanceService);
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
            CashAdvance model = new CashAdvance();
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
                model = _cashAdvanceService.GetObjectById(id);
                int userId = AuthenticationModel.GetUserId();
                DateTime ConfirmationDate = DateTime.Now;
                model = _cashAdvanceService.ConfirmObject(model, ConfirmationDate, _cashAdvanceDetailService);
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
            CashAdvance model = new CashAdvance();
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
                model = _cashAdvanceService.GetObjectById(id);
                int userId = AuthenticationModel.GetUserId();
                model = _cashAdvanceService.UnconfirmObject(model,_cashAdvanceDetailService);
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

                var query = _cashAdvanceDetailService.GetQueryable().Where(x => x.CashAdvanceId == Id && x.IsDeleted == false).ToList();
                var listdata = (from data in query
                                select new
                                {
                                    ContactName = data.CashAdvance.Employee.Name,
                                    ContactAddress = "",
                                    PrintDate = DateTime.Today.Date,
                                    PRNo = data.CashAdvance.CashAdvanceNo,
                                    ShipmentOrderNo = data.ShipmentNo,
                                    Description = data.Description, 
                                    AmountIDR = data.AmountIDR ,
                                    AmountUSD = data.AmountUSD,

                                }).ToList();
                rd.Load(Server.MapPath("~/") + "Reports/CashAdvance.rpt");

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
