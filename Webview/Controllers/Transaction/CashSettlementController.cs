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
    public class CashSettlementController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("CashSettlementController");
        private ICashSettlementService _cashSettlementService;
        private IExchangeRateService _exchangeRateService;
        private IShipmentOrderService _shipmentOrderService;
        private IAccountUserService _accountUserService;
        private IContactService _contactService;
        private IOfficeService _officeService;
        public IVatService _vatService;
        private IPayableService _payableService;
        private ICashBankService _cashBankService;
        private IPaymentRequestService _paymentRequestService;
        private ICashMutationService _cashMutationService;

        public CashSettlementController()
        {
            _cashSettlementService = new CashSettlementService(new CashSettlementRepository(), new CashSettlementValidation());
            _shipmentOrderService = new ShipmentOrderService(new ShipmentOrderRepository(), new ShipmentOrderValidation());
            _officeService = new OfficeService(new OfficeRepository(),new OfficeValidation());
            _accountUserService = new AccountUserService(new AccountUserRepository(), new AccountUserValidator());
            _contactService = new ContactService(new ContactRepository(), new ContactValidation());
            _exchangeRateService = new ExchangeRateService(new ExchangeRateRepository(), new ExchangeRateValidation());
            _vatService = new VatService(new VatRepository(), new VatValidation());
            _payableService = new PayableService(new PayableRepository(), new PayableValidator());
            _cashBankService = new CashBankService(new CashBankRepository(), new CashBankValidator());
            _paymentRequestService = new PaymentRequestService(new PaymentRequestRepository(), new PaymentRequestValidation());
            _cashMutationService = new CashMutationService(new CashMutationRepository(), new CashMutationValidator());
        }


        public ActionResult Index()
        {
            //if (!AuthenticationModel.IsAllowed("View", Core.Constants.Constant.MenuName.CashSettlement, Core.Constants.Constant.MenuGroupName.Master))
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
            var q = _cashSettlementService.GetQueryable().Where(x => x.OfficeId == officeid); 

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.IsDeleted,
                             model.IsConfirmed,
                             model.SettlementNo,
                             EmployeeName = model.Employee.Name,
                             CashAdvanceNo = model.CashAdvance.CashAdvanceNo,
                             model.SettlementUSD,
                             model.SettlementIDR,
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
                             model.SettlementNo,
                             model.EmployeeName,
                             model.CashAdvanceNo,
                             model.SettlementUSD,
                             model.SettlementIDR,
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
            CashSettlement model = new CashSettlement();
            try
            {
                model = _cashSettlementService.GetObjectById(Id);

                return Json(new
                {
                    model.Id,
                    model.CashAdvanceId,
                    model.SettlementNo,
                    model.SettlementIDR,
                    model.SettlementUSD,
                    model.Reference,
                    model.Rate,
                    model.ExRateDate,
                    model.PrintedAt,
                    CashAdvanceNo = model.CashAdvance.CashAdvanceNo,
                    CashAdvanceIDR = model.CashAdvance.CashAdvanceIDR,
                    CashAdvanceUSD = model.CashAdvance.CashAdvanceUSD,
                    model.EmployeeId,
                    EmployeeName = model.Employee.Name,
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
        public dynamic Insert(CashSettlement model)
        {
            ////try
            {
                //if (!AuthenticationModel.IsAllowed("Create", Core.Constants.Constant.MenuName.CashSettlement, Core.Constants.Constant.MenuGroupName.Master))
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
                model = _cashSettlementService.CreateObject(model);
                
                return Json(new
                {
                    model.Errors,
                    cashSettlementId = model.Id,
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
        public dynamic Update(CashSettlement model)
        {
            try
            {
                //if (!AuthenticationModel.IsAllowed("Edit", Core.Constants.Constant.MenuName.CashSettlement, Core.Constants.Constant.MenuGroupName.Master))
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
                var data = _cashSettlementService.GetObjectById(model.Id);
                model = _cashSettlementService.UpdateObject(data);
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
        public dynamic Delete(CashSettlement model)
        {
            try
            {
                //if (!AuthenticationModel.IsAllowed("Delete", Core.Constants.Constant.MenuName.CashSettlement, Core.Constants.Constant.MenuGroupName.Master))
                //{
                //    Dictionary<string, string> Errors = new Dictionary<string, string>();
                //    Errors.Add("Generic", "You are Not Allowed to Delete Record");

                //    return Json(new
                //    {
                //        Errors
                //    }, JsonRequestBehavior.AllowGet);
                //}

                var data = _cashSettlementService.GetObjectById(model.Id);
                int userId = AuthenticationModel.GetUserId();
                data.OfficeId = _accountUserService.GetObjectById(userId).OfficeId;
                model = _cashSettlementService.SoftDeleteObject(data);
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
            CashSettlement model = new CashSettlement();
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
                model = _cashSettlementService.GetObjectById(id);
                int userId = AuthenticationModel.GetUserId();
                DateTime ConfirmationDate = DateTime.Now;
               // model = _cashSettlementService;
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
            CashSettlement model = new CashSettlement();
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
                model = _cashSettlementService.GetObjectById(id);
                int userId = AuthenticationModel.GetUserId();
                //model = _cashSettlementService.UnconfirmObject(model,_cashSettlementDetailService);
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

                var query = _cashSettlementService.GetQueryable().Where(x => x.Id == Id && x.IsDeleted == false).ToList();
                var datareport = query.FirstOrDefault();
                var SettlementIDR = datareport.SettlementIDR ?? 0;
                var SettlementUSD = datareport.SettlementUSD ?? 0;
                string WordTotalamountIDR = SettlementIDR == 0 ? "0" : GeneralFunction.changeCurrencyToWords(SettlementIDR, true, "IDR", "");
                string WordTotalamountUSD = SettlementUSD == 0 ? "0" : GeneralFunction.changeCurrencyToWords(SettlementUSD, true, "USD", "");
                string Says;
                Says = WordTotalamountUSD == "0" ? "" : WordTotalamountUSD;
                Says = Says == "" ? WordTotalamountIDR : WordTotalamountUSD + (WordTotalamountIDR == "0" ? "" : " PLUS " + WordTotalamountIDR);
                var listdata = (from data in query
                                select new
                                {
                                    Reference = data.Reference,
                                    DateSettl = DateTime.Today.Date,
                                    ReceiptFrom = data.SettlementNo,
                                    SettlementNo = data.CashAdvance.Reference,
                                    InUSD = data.SettlementUSD ?? 0,
                                    InIDR = data.SettlementIDR ?? 0,
                                    BeginUSD = data.CashAdvance.CashAdvanceUSD,
                                    BeginIDR = data.CashAdvance.CashAdvanceIDR,
                                    SetUSD = data.SettlementUSD ?? 0,
                                    SetIDR = data.SettlementIDR ?? 0,
                                    CompanyCode = "",
                                    Says = Says
                                }).ToList();
                rd.Load(Server.MapPath("~/") + "Reports/Settlement.rpt");

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
