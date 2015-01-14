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
using System.Data.Objects.SqlClient;
using Data.Context;
using CrystalDecisions.CrystalReports.Engine;

namespace WebView.Controllers
{
    public class PaymentRequestController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("PaymentRequestController");
        private IPaymentRequestService _paymentRequestService;
        private IExchangeRateService _exchangeRateService;
        private IPaymentRequestDetailService _paymentRequestDetailService;
        private IShipmentOrderService _shipmentOrderService;
        private IAccountUserService _accountUserService;
        private IDepoService _depoService;
        private IContainerYardService _containerYardService;
        private IEmployeeService _employeeService;
        private ITruckService _truckService;
        private IContactService _contactService;
        private IOfficeService _officeService;
        public IVatService _vatService;
        private IPayableService _payableService;
        private IPaymentVoucherDetailService _paymentVoucherDetailService;

        public PaymentRequestController()
        {
            _paymentRequestService = new PaymentRequestService(new PaymentRequestRepository(), new PaymentRequestValidation());
            _paymentRequestDetailService = new PaymentRequestDetailService(new PaymentRequestDetailRepository(), new PaymentRequestDetailValidation());
            _shipmentOrderService = new ShipmentOrderService(new ShipmentOrderRepository(), new ShipmentOrderValidation());
            _officeService = new OfficeService(new OfficeRepository(),new OfficeValidation());
            _accountUserService = new AccountUserService(new AccountUserRepository(), new AccountUserValidator());
            _depoService = new DepoService(new DepoRepository(), new DepoValidation());
            _containerYardService = new ContainerYardService(new ContainerYardRepository(), new ContainerYardValidation());
            _employeeService = new EmployeeService(new EmployeeRepository(), new EmployeeValidation());
            _truckService = new TruckService(new TruckRepository(), new TruckValidation());
            _contactService = new ContactService(new ContactRepository(), new ContactValidation());
            _exchangeRateService = new ExchangeRateService(new ExchangeRateRepository(), new ExchangeRateValidation());
            _vatService = new VatService(new VatRepository(), new VatValidation());
            _payableService = new PayableService(new PayableRepository(), new PayableValidator());

        }


        public ActionResult Index()
        {
            //if (!AuthenticationModel.IsAllowed("View", Core.Constants.Constant.MenuName.PaymentRequest, Core.Constants.Constant.MenuGroupName.Master))
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
            var q = _paymentRequestService.GetQueryable().Where(x => x.OfficeId == officeid && x.ShipmentOrder.JobId == JobId); //.Include("PaymentRequest").Include("UoM");

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.IsDeleted,
                             model.IsConfirmed,
                             JenisPaymentRequest = model.JenisPaymentRequest == "G" ? "General" : "PR",
                             model.PRNo,
                             Paid =  model.Paid == null ? false : model.Paid,    
                             ShipmentOrderCode =  model.ShipmentOrder.ShipmentOrderCode,
                             model.PRStatus,
                             model.DebetCredit,
                             LinkTo = model.PRContraNo != null ? (model.PRContraStatus + SqlFunctions.StringConvert((double)model.PRContraNo)) : "",
                             Contact = model.Contact.ContactName,
                             model.PaymentUSD,
                             model.PaymentIDR,
                             model.PrintedAt,
                             model.Rate,
                             model.ExRateDate,
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
                             model.IsDeleted,
                             model.IsConfirmed,
                             model.JenisPaymentRequest,
                             model.PRNo,
                             model.Paid,
                             model.ShipmentOrderCode,
                             model.PRStatus,
                             model.DebetCredit,
                             model.LinkTo,
                             model.Contact,
                             model.PaymentUSD,
                             model.PaymentIDR,
                             model.PrintedAt,
                             model.Rate,
                             model.ExRateDate,
                             model.CreatedAt,
                             model.CreatedBy,
                             model.Printing,
                        }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }
         
        public dynamic GetLookUpPV(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "",
                                                int CustomerId = 0, int CurrencyId = 0)
        {
            // Construct where statement
            int officeid = _accountUserService.GetObjectById(AuthenticationModel.GetUserId()).OfficeId;

            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _paymentRequestService.GetQueryable().Where(x => x.OfficeId == officeid && x.IsConfirmed == true && 
                (!x.Paid.HasValue || x.Paid.Value == false) && x.ContactId == CustomerId && x.CurrencyId == CurrencyId);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.PRNo,
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
                             model.PRNo,
                             model.ShipmentOrderCode,
                             model.PaymentUSD,
                             model.PaymentIDR,
                        }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
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
                //Invoice model =   .Print(Id, fd);

                //if (model.Errors.Any())
                //{
                //    return Content(model.Errors.FirstOrDefault().Value.ToString());
                //}
                //else
                //{
                    var query = _paymentRequestDetailService.GetQueryable().Where(x=>x.PaymentRequestId == Id && x.IsDeleted == false);
                    var listdata = (from data in query
                                    select new
                                    {
                                        ContactName = data.PaymentRequest.Contact.ContactName,
                                        ContactAddress = data.PaymentRequest.Contact.ContactAddress,
                                        PrintDate = DateTime.Today.Date,
                                        PRNo = data.PaymentRequest.PRNo, 
                                        ShipmentOrderNo = data.PaymentRequest.ShipmentOrder.ShipmentOrderCode,
                                        Reference = data.PaymentRequest.Reference,
                                        Description = data.Description,
                                        Amount = data.Amount.Value,
                                    }).ToList();
                    rd.Load(Server.MapPath("~/") + "Reports/PaymentRequest.rpt");

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


        public dynamic GetLookUpCA(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "",
                                                int CustomerId = 0, int CurrencyId = 0)
        {
            // Construct where statement
            int officeid = _accountUserService.GetObjectById(AuthenticationModel.GetUserId()).OfficeId;

            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _paymentRequestService.GetQueryable().Where(x => x.OfficeId == officeid && x.IsConfirmed == true);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.PRNo,
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
                             model.PRNo,
                             model.ShipmentOrderCode,
                             model.PaymentUSD,
                             model.PaymentIDR,
                        }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        //Payable
        public dynamic GetLookUpPVDetail(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "",
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
            int PRNo = _paymentRequestService.GetObjectById(PRId).PRNo;
            using (var db = new ExpedicoEntities())
            {
                var query = (from a in db.Payables
                             where a.PayableSource == MasterConstant.SourceDocument.PaymentRequest && a.PayableSourceId == PRId 
                             && a.IsDeleted == false
                             select new 
                             {
                               Id = a.Id,
                               PRNo = PRNo,
                               AmountIDR = a.CurrencyId == MasterConstant.Currency.IDR ? a.Amount : 0,
                               RemainingAmountIDR = a.CurrencyId == MasterConstant.Currency.IDR ? a.RemainingAmount : 0,
                               AmountUSD = a.CurrencyId == MasterConstant.Currency.USD ? a.Amount : 0,
                               RemainingAmountUSD = a.CurrencyId == MasterConstant.Currency.USD ? a.RemainingAmount : 0,
                               Description = db.PaymentRequestDetails.Where(x => x.Id == a.PayableSourceDetailId).FirstOrDefault().Description,
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
                             model.PRNo,
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
              //  objJob.VesselTypeList = _paymentRequestService.GetVesselTypeList();
                
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
            PaymentRequest model = new PaymentRequest();
            try
            {
                model = _paymentRequestService.GetObjectById(Id);

                return Json(new
                {
                    model.Id,
                    model.ShipmentOrderId,
                    model.PRNo,
                    LinkTo = model.PRContraNo == null ? "" : model.PRContraStatus + model.PRContraNo.ToString() ,
                    ShipmentOrderCode = model.ShipmentOrder.ShipmentOrderCode,
                    model.ShipmentOrder.JobNumber,
                    model.ShipmentOrder.SubJobNumber,
                    model.ShipmentOrder.JobId,
                    model.OfficeId,
                    model.Office.InitialCompany,
                    model.DebetCredit,
                    model.JenisPaymentRequest,
                    model.CurrencyId,
                    model.ContactId,
                    CustomerCode = model.Contact.MasterCode,
                    CustomerName = model.Contact.ContactName,
                    CustomerAddress = model.Contact.ContactAddress,
                    model.PaymentIDR,
                    model.PaymentUSD,
                    model.Rate,
                    model.ExRateDate,
                    model.PrintedAt,
                    model.Printing,
                    ETDETA = model.ShipmentOrder.ETD,
                    ListPaymentRequestDetail = (from detail in model.PaymentRequestDetail
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
                                         }).ToArray(),

                    model.Errors,
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
        public dynamic Insert(PaymentRequest model)
        {
            ////try
            {
                //if (!AuthenticationModel.IsAllowed("Create", Core.Constants.Constant.MenuName.PaymentRequest, Core.Constants.Constant.MenuGroupName.Master))
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
                model = _paymentRequestService.CreateObject(model,_shipmentOrderService);
                
                return Json(new
                {
                    model.Errors,
                    paymentRequestId = model.Id,
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
        public dynamic InsertDetail(PaymentRequestDetail model)
        {
            ////try
            {
                //if (!AuthenticationModel.IsAllowed("Create", Core.Constants.Constant.MenuName.PaymentRequest, Core.Constants.Constant.MenuGroupName.Master))
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
                model = _paymentRequestDetailService.CreateObject(model, _paymentRequestService);

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
        public dynamic Update(PaymentRequest model)
        {
            try
            {
                //if (!AuthenticationModel.IsAllowed("Edit", Core.Constants.Constant.MenuName.PaymentRequest, Core.Constants.Constant.MenuGroupName.Master))
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
                var data = _paymentRequestService.GetObjectById(model.Id);
                model = _paymentRequestService.UpdateObject(data,_shipmentOrderService);
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
        public dynamic UpdateDetail(PaymentRequestDetail model)
        {
            try
            {
                //if (!AuthenticationModel.IsAllowed("Edit", Core.Constants.Constant.MenuName.PaymentRequest, Core.Constants.Constant.MenuGroupName.Master))
                //{
                //    Dictionary<string, string> Errors = new Dictionary<string, string>();
                //    Errors.Add("Generic", "You are Not Allowed to Edit record");

                //    return Json(new
                //    {
                //        Errors
                //    }, JsonRequestBehavior.AllowGet);
                //}

                int userId = AuthenticationModel.GetUserId();
                var data = _paymentRequestDetailService.GetObjectById(model.Id);
                data.UpdatedById = userId;
                data.OfficeId = _accountUserService.GetObjectById(userId).OfficeId;
                data.EPLDetailId = model.EPLDetailId;
                data.CostId = model.CostId;
                data.Description = model.Description;
                data.Quantity = model.Quantity;
                data.PerQty = model.PerQty;
                data.Type = model.Type;
                data.CodingQuantity = model.CodingQuantity;
                data.AmountCrr = model.AmountCrr;
                data.Amount = model.Amount;
                model = _paymentRequestDetailService.UpdateObject(data,_paymentRequestService);
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
        public dynamic Delete(PaymentRequest model)
        {
            try
            {
                //if (!AuthenticationModel.IsAllowed("Delete", Core.Constants.Constant.MenuName.PaymentRequest, Core.Constants.Constant.MenuGroupName.Master))
                //{
                //    Dictionary<string, string> Errors = new Dictionary<string, string>();
                //    Errors.Add("Generic", "You are Not Allowed to Delete Record");

                //    return Json(new
                //    {
                //        Errors
                //    }, JsonRequestBehavior.AllowGet);
                //}

                var data = _paymentRequestService.GetObjectById(model.Id);
                int userId = AuthenticationModel.GetUserId();
                data.OfficeId = _accountUserService.GetObjectById(userId).OfficeId;
                model = _paymentRequestService.SoftDeleteObject(data);
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
        public dynamic DeleteDetail(PaymentRequestDetail model)
        { 
            try
            {
                //if (!AuthenticationModel.IsAllowed("Delete", Core.Constants.Constant.MenuName.PaymentRequest, Core.Constants.Constant.MenuGroupName.Master))
                //{
                //    Dictionary<string, string> Errors = new Dictionary<string, string>();
                //    Errors.Add("Generic", "You are Not Allowed to Delete Record");

                //    return Json(new
                //    {
                //        Errors
                //    }, JsonRequestBehavior.AllowGet);
                //}

                var data = _paymentRequestDetailService.GetObjectById(model.Id);
                int userId = AuthenticationModel.GetUserId();
                data.CreatedById = userId;
                data.OfficeId = _accountUserService.GetObjectById(userId).OfficeId;
                model = _paymentRequestDetailService.SoftDeleteObject(data,_paymentRequestService);
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
            PaymentRequest model = new PaymentRequest();
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
                model = _paymentRequestService.GetObjectById(id);
                int userId = AuthenticationModel.GetUserId();
                model.OfficeId = _accountUserService.GetObjectById(userId).OfficeId;
                DateTime ConfirmationDate = DateTime.Now;
                model = _paymentRequestService.ConfirmObject(model, ConfirmationDate, _paymentRequestDetailService, _payableService);
            }
            catch (Exception ex)
            {
                LOG.Error("Confirm Failed", ex);
                Dictionary<string, string> Errors = new Dictionary<string, string>();
                Errors.Add("Generic", "Error " + ex);

                return Json(new
                {
                    model.Errors
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
            PaymentRequest model = new PaymentRequest();
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
                model = _paymentRequestService.GetObjectById(id);
                int userId = AuthenticationModel.GetUserId();
                model.OfficeId = _accountUserService.GetObjectById(userId).OfficeId;
                DateTime ConfirmationDate = DateTime.Now;
                model = _paymentRequestService.UnconfirmObject(model,_paymentRequestDetailService,_payableService,_paymentVoucherDetailService);
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

    }
}
