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

namespace WebView.Controllers
{
    public class EstimateProfitLossController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("EstimateProfitLossController");
        private IEstimateProfitLossService _estimateProfitLossService;
        private IEstimateProfitLossDetailService _estimateProfitLossDetailService;
        private IShipmentOrderService _shipmentOrderService;
        private IAccountUserService _accountUserService;
        private IDepoService _depoService;
        private IContainerYardService _containerYardService;
        private IEmployeeService _employeeService;
        private ITruckService _truckService;
        private IContactService _contactService;
        private IOfficeService _officeService;
        private IExchangeRateService _exchangeRateService;
        private ISeaContainerService _seaContainerService;
        private ICostService _costService;
        private IPaymentRequestDetailService _paymentRequestDetailService;
        private IInvoiceDetailService _invoiceDetailService;

        public EstimateProfitLossController()
        {
            _estimateProfitLossService = new EstimateProfitLossService(new EstimateProfitLossRepository(), new EstimateProfitLossValidation());
            _estimateProfitLossDetailService = new EstimateProfitLossDetailService(new EstimateProfitLossDetailRepository(), new EstimateProfitLossDetailValidation());
            _officeService = new OfficeService(new OfficeRepository(),new OfficeValidation());
            _accountUserService = new AccountUserService(new AccountUserRepository(), new AccountUserValidator());
            _depoService = new DepoService(new DepoRepository(), new DepoValidation());
            _containerYardService = new ContainerYardService(new ContainerYardRepository(), new ContainerYardValidation());
            _employeeService = new EmployeeService(new EmployeeRepository(), new EmployeeValidation());
            _truckService = new TruckService(new TruckRepository(), new TruckValidation());
            _contactService = new ContactService(new ContactRepository(), new ContactValidation());
            _shipmentOrderService = new ShipmentOrderService(new ShipmentOrderRepository(), new ShipmentOrderValidation());
            _exchangeRateService = new ExchangeRateService(new ExchangeRateRepository(), new ExchangeRateValidation());
            _seaContainerService = new SeaContainerService(new SeaContainerRepository(), new SeaContainerValidation());
            _costService = new CostService(new CostRepository(), new CostValidation());
            _paymentRequestDetailService = new PaymentRequestDetailService(new PaymentRequestDetailRepository(), new PaymentRequestDetailValidation());
            _invoiceDetailService = new InvoiceDetailService(new InvoiceDetailRepository(), new InvoiceDetailValidation());
        }


        public ActionResult Index()
        {
            //if (!AuthenticationModel.IsAllowed("View", Core.Constants.Constant.MenuName.EstimateProfitLoss, Core.Constants.Constant.MenuGroupName.Master))
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
            var q = _estimateProfitLossService.GetQueryable().Where(x => x.OfficeId == officeid && x.ShipmentOrder.JobId == JobId); //.Include("EstimateProfitLoss").Include("UoM");

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.IsConfirmed,
                             ShipmentOrderCode = model.ShipmentOrder.ShipmentOrderCode,
                             ETDETA = model.ShipmentOrder.ETD,
                             model.TotalCostIDR,
                             model.TotalCostUSD,
                             model.TotalIncomeUSD,
                             model.TotalIncomeIDR,
                             TotalUSD = model.TotalIncomeUSD - model.TotalCostUSD,
                             TotalIDR = model.TotalIncomeIDR - model.TotalCostIDR,
                             model.DateClose,
                             model.Printing,
                             model.PrintDate,
                             model.IsDeleted,
                             model.CreatedAt,
                             CreatedBy = model.CreatedBy.Name,
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
                          model.ShipmentOrderCode,
                          model.ETDETA,
                          model.TotalUSD,
                          model.TotalIDR,
                          model.TotalIncomeUSD,
                          model.TotalCostUSD,
                          model.TotalIncomeIDR,
                          model.TotalCostIDR,
                          model.DateClose,
                          model.Printing,
                          model.PrintDate,
                          model.CreatedAt,
                          model.CreatedBy,
                          model.UpdatedAt,
                        }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }
         
        public dynamic GetLookUpIncome(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "",
                                               int shipmentOrderId = 0,int customerId = 0,int amountCrr = MasterConstant.Currency.IDR)
        { 
            // Construct where statement
            int officeid = _accountUserService.GetObjectById(AuthenticationModel.GetUserId()).OfficeId;
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _estimateProfitLossDetailService.GetQueryable().Where(x => x.EstimateProfitLoss.ShipmentOrderId == shipmentOrderId
                && x.ContactId == customerId && x.IsIncome == true && x.IsDeleted == false && x.IsConfirmed == false
                && x.AmountCrr == amountCrr);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.CostId,
                             model.Cost.MasterCode,
                             Contact = model.Contact.ContactName,
                             model.Description,
                             model.AmountUSD,
                             model.AmountIDR,
                             model.CodingQuantity,
                             CurrencyType = model.AmountCrr == 0 ? "USD" : "IDR",
                             Payment = model.AmountCrr == 0 ? model.AmountUSD : model.AmountIDR,
                             model.Type,
                             model.Quantity,
                             model.PerQty,  
                             model.IsDeleted,
                             model.CreatedAt,
                             CreatedBy = model.CreatedBy.Name,
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
                          model.MasterCode,
                             model.Contact,
                             model.Description,
                             model.AmountUSD,
                             model.AmountIDR,
                             model.CodingQuantity,
                             model.CurrencyType,
                             model.Payment,
                             model.Type,
                             model.Quantity,
                             model.PerQty,  
                             model.Id,
                             model.CostId,
                        }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetLookUpCost(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "",
                                            int shipmentOrderId = 0, int customerId = 0, int amountCrr = MasterConstant.Currency.IDR)
        { 
            // Construct where statement
            int officeid = _accountUserService.GetObjectById(AuthenticationModel.GetUserId()).OfficeId;
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _estimateProfitLossDetailService.GetQueryable().Where(x => x.EstimateProfitLoss.ShipmentOrderId == shipmentOrderId
                && x.ContactId == customerId && x.IsIncome == false && x.IsDeleted == false && x.IsConfirmed == true
                && x.AmountCrr == amountCrr);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.CostId,
                             model.Cost.MasterCode,
                             Contact = model.Contact.ContactName,
                             model.Description,
                             model.AmountUSD,
                             model.AmountIDR,
                             model.CodingQuantity,
                             CurrencyType = model.AmountCrr == 0 ? "USD" : "IDR",
                             Payment = model.AmountCrr == 0 ? model.AmountUSD : model.AmountIDR,
                             model.Type,
                             model.Quantity,
                             model.PerQty,
                             model.IsDeleted,
                             model.CreatedAt,
                             CreatedBy = model.CreatedBy.Name,
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
                          model.MasterCode,
                             model.Contact,
                             model.Description,
                             model.AmountUSD,
                             model.AmountIDR,
                             model.CodingQuantity,
                             model.CurrencyType,
                             model.Payment,
                             model.Type,
                             model.Quantity,
                             model.PerQty,  
                             model.Id,
                             model.CostId,
                        }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
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

            EstimateProfitLoss objJob = new EstimateProfitLoss();
            try
            {
               // objJob.CurrencyList = (from c in _countryService.GetCurrencyList() where c != "" orderby c select c).Distinct().ToList();
              //  objJob.JobOwnerList = (from j in _jobownerService.GetJobOwner("", "", "") select j).Distinct().ToList();
              //  objJob.VesselTypeList = _estimateProfitLossService.GetVesselTypeList();

                switch (JobId)
                {
                    case MasterConstant.JobType.SeaExport:
                        ViewBag.Title = "EPL - Sea Export";
                        return View("SeaExportDetail", objJob);
                    case MasterConstant.JobType.SeaImport:
                        ViewBag.Title = "EPL - Sea Import";
                        return View("SeaImportDetail", objJob);
                    case MasterConstant.JobType.AirExport:
                        ViewBag.Title = "EPL - Air Export";
                        return View("AirExportDetail", objJob);
                    case MasterConstant.JobType.AirImport:
                        ViewBag.Title = "EPL - Air Import";
                        return View("AirImportDetail", objJob);
                    case MasterConstant.JobType.EMKLSeaExport:
                        ViewBag.Title = "EPL - EMKL Sea Export";
                        return View("EMKLSeaExportDetail", objJob);
                    case MasterConstant.JobType.EMKLSeaImport:
                        ViewBag.Title = "EPL - EMKL Sea Import";
                        return View("EMKLSeaImportDetail", objJob);
                    case MasterConstant.JobType.EMKLAirExport:
                        ViewBag.Title = "EPL - EMKL Air Export";
                        return View("EMKLAirExportDetail", objJob);
                    case MasterConstant.JobType.EMKLAirImport:
                        ViewBag.Title = "EPL - EMKL Air Import";
                        return View("EMKLAirImportDetail", objJob);
                    case MasterConstant.JobType.EMKLDomestic:
                        ViewBag.Title = "EPL - EMKL Domestic";
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
            EstimateProfitLoss model = new EstimateProfitLoss();
            try
            {
                model = _estimateProfitLossService.GetObjectById(Id);

                return Json(new
                {
                    model.Id,
                    model.ShipmentOrderId,
                    ShipmentNo = model.ShipmentOrder.ShipmentOrderCode,
                    ETD = model.ShipmentOrder.ETD,
                    ConsigneeId = model.ShipmentOrder.ConsigneeId,
                    ConsigneeCode = model.ShipmentOrder.Consignee != null ? model.ShipmentOrder.Consignee.MasterCode.ToString() : "",
                    ConsigneeName = model.ShipmentOrder.ConsigneeName,
                    AgentId = model.ShipmentOrder.AgentId,
                    AgentCode = model.ShipmentOrder.Agent != null ? model.ShipmentOrder.Agent.MasterCode.ToString() : "",
                    AgentName = model.ShipmentOrder.AgentName,
                    JobEntryDate = model.ShipmentOrder.CreatedAt,
                    model.DateClose,
                    model.CloseEPL,
                    IncomeList = (from detail in model.EstimateProfitLossDetails
                                  where detail.IsDeleted == false && detail.IsIncome == true
                                  select new
                                  {
                                      detail.Id,
                                      ContactId = detail.ContactId,
                                      ContactCode = detail.Contact != null ? detail.Contact.MasterCode.ToString() : "",
                                      ContactName = detail.Contact != null ? detail.Contact.ContactName : "",
                                      detail.CostId,
                                      CostName = detail.Cost != null ? detail.Cost.Name : "",
                                      detail.Description,
                                      detail.Type,
                                      detail.AmountCrr,
                                      detail.AmountUSD,
                                      detail.AmountIDR,
                                      detail.Quantity,
                                      detail.PerQty,
                                      detail.CodingQuantity,
                                      detail.Sign
                                  }).ToArray(),
                    CostList = (from detail in model.EstimateProfitLossDetails
                                  where detail.IsDeleted == false && detail.IsIncome == false
                                  select new
                                  {
                                      detail.Id,
                                      ContactId = detail.ContactId,
                                      ContactCode = detail.Contact != null ? detail.Contact.MasterCode.ToString() : "",
                                      ContactName = detail.Contact != null ? detail.Contact.ContactName : "",
                                      detail.CostId,
                                      CostName = detail.Cost != null ? detail.Cost.Name : "",
                                      detail.Description,
                                      detail.Type,
                                      detail.AmountCrr,
                                      detail.AmountUSD,
                                      detail.AmountIDR,
                                      detail.Quantity,
                                      detail.PerQty,
                                      detail.CodingQuantity,
                                      detail.Sign
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

        public dynamic ValidateEPL(int Id)
        {
            EstimateProfitLossDetail model = new EstimateProfitLossDetail();
            try
            {
                model = _estimateProfitLossDetailService.ValidateEPL(Id,_paymentRequestDetailService,_invoiceDetailService);
                return Json(new
                {
                    model.Errors
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LOG.Error("GetInfo", ex);
                Dictionary<string, string> Errors = new Dictionary<string, string>();
                model.Errors.Add("Generic", "Error " + ex);

                return Json(new
                {
                    model.Errors
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public dynamic Insert(EstimateProfitLoss model)
        {
            ////try
            {
                //if (!AuthenticationModel.IsAllowed("Create", Core.Constants.Constant.MenuName.EstimateProfitLoss, Core.Constants.Constant.MenuGroupName.Master))
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
                model = _estimateProfitLossService.CreateObject(model,_shipmentOrderService);
                
                return Json(new
                { 
                    model.Errors,
                    eplId = model.Id,
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
        public dynamic InsertDetail(EstimateProfitLossDetail model)
        {
            ////try 
            {
                //if (!AuthenticationModel.IsAllowed("Create", Core.Constants.Constant.MenuName.EstimateProfitLoss, Core.Constants.Constant.MenuGroupName.Master))
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
                model = _estimateProfitLossDetailService.CreateObject(model,_shipmentOrderService,_seaContainerService,_estimateProfitLossService,_contactService,_costService);

                return Json(new
                {
                    model.Errors,
                    eplId = model.Id,
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
        public dynamic Update(EstimateProfitLoss model)
        {
            try
            {
                //if (!AuthenticationModel.IsAllowed("Edit", Core.Constants.Constant.MenuName.EstimateProfitLoss, Core.Constants.Constant.MenuGroupName.Master))
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
                var data = _estimateProfitLossService.GetObjectById(model.Id);
                model = _estimateProfitLossService.UpdateObject(data,_shipmentOrderService);
            }
            catch (Exception ex)
            {
                LOG.Error("Update Failed", ex);
                Dictionary<string, string> Errors = new Dictionary<string, string>();
                model.Errors.Add("Generic", "Update Failed " + ex);

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
        public dynamic UpdateDetail(EstimateProfitLossDetail model)
        { 
            try
            {
                //if (!AuthenticationModel.IsAllowed("Edit", Core.Constants.Constant.MenuName.EstimateProfitLoss, Core.Constants.Constant.MenuGroupName.Master))
                //{
                //    Dictionary<string, string> Errors = new Dictionary<string, string>();
                //    Errors.Add("Generic", "You are Not Allowed to Edit record");

                //    return Json(new
                //    {
                //        Errors
                //    }, JsonRequestBehavior.AllowGet);
                //}

                int userId = AuthenticationModel.GetUserId();
                var data = _estimateProfitLossDetailService.GetObjectById(model.Id);
                data.UpdatedById = userId;
                data.OfficeId = _accountUserService.GetObjectById(userId).OfficeId;
                data.PerQty = model.PerQty;
                data.Quantity = model.Quantity;
                data.CodingQuantity = model.CodingQuantity;
                data.IsIncome = model.IsIncome;
                data.Sign = model.Sign;
                data.AmountIDR = model.AmountIDR;
                data.AmountUSD = model.AmountUSD;

                model = _estimateProfitLossDetailService.UpdateObject(data, _estimateProfitLossService, _contactService, _costService);
            }
            catch (Exception ex)
            {
                LOG.Error("Update Failed", ex);
                Dictionary<string, string> Errors = new Dictionary<string, string>();
                model.Errors.Add("Generic", "Update Failed " + ex);

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
        public dynamic Delete(EstimateProfitLoss model)
        {
            try
            {
                //if (!AuthenticationModel.IsAllowed("Delete", Core.Constants.Constant.MenuName.EstimateProfitLoss, Core.Constants.Constant.MenuGroupName.Master))
                //{
                //    Dictionary<string, string> Errors = new Dictionary<string, string>();
                //    Errors.Add("Generic", "You are Not Allowed to Delete Record");

                //    return Json(new
                //    {
                //        Errors
                //    }, JsonRequestBehavior.AllowGet);
                //}

                var data = _estimateProfitLossService.GetObjectById(model.Id);
                int userId = AuthenticationModel.GetUserId();
                data.CreatedById = userId;
                data.OfficeId = _accountUserService.GetObjectById(userId).OfficeId;
                model = _estimateProfitLossService.SoftDeleteObject(data,_estimateProfitLossDetailService);
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
        public dynamic DeleteDetail(EstimateProfitLossDetail model)
        {
            try
            {
                //if (!AuthenticationModel.IsAllowed("Delete", Core.Constants.Constant.MenuName.EstimateProfitLoss, Core.Constants.Constant.MenuGroupName.Master))
                //{
                //    Dictionary<string, string> Errors = new Dictionary<string, string>();
                //    Errors.Add("Generic", "You are Not Allowed to Delete Record");

                //    return Json(new
                //    {
                //        Errors
                //    }, JsonRequestBehavior.AllowGet);
                //}

                var data = _estimateProfitLossDetailService.GetObjectById(model.Id);
                int userId = AuthenticationModel.GetUserId();
                data.CreatedById = userId;
                data.OfficeId = _accountUserService.GetObjectById(userId).OfficeId;
                model = _estimateProfitLossDetailService.SoftDeleteObject(data);
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
            EstimateProfitLoss model = new EstimateProfitLoss();
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
                model = _estimateProfitLossService.GetObjectById(id);
                int userId = AuthenticationModel.GetUserId();
                DateTime ConfirmationDate = DateTime.Now;
                model = _estimateProfitLossService.ConfirmObject(model, ConfirmationDate, _estimateProfitLossDetailService);
               
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
            EstimateProfitLoss model = new EstimateProfitLoss();
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
                model = _estimateProfitLossService.GetObjectById(id);
                int userId = AuthenticationModel.GetUserId();
                DateTime ConfirmationDate = DateTime.Now;
                model = _estimateProfitLossService.UnconfirmObject(model,ConfirmationDate,_estimateProfitLossDetailService,_invoiceDetailService,_paymentRequestDetailService);

            }
            catch (Exception ex)
            {
                LOG.Error("Unconfirm Failed", ex);
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
