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
    public class ShipmentOrderController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("ShipmentOrderController");
        private IShipmentOrderService _shipmentOrderService;
        private IEstimateProfitLossService _estimateProfitLossService;
        private IAccountUserService _accountUserService;
        private IDepoService _depoService;
        private IContainerYardService _containerYardService;
        private IEmployeeService _employeeService;
        private ITruckService _truckService;
        private IContactService _contactService;
        private IOfficeService _officeService;
        private IShipmentOrderDocumentService _shipmentOrderDocumentService;

        public ShipmentOrderController()
        {
            _shipmentOrderService = new ShipmentOrderService(new ShipmentOrderRepository(), new ShipmentOrderValidation());
            _officeService = new OfficeService(new OfficeRepository(),new OfficeValidation());
            _accountUserService = new AccountUserService(new AccountUserRepository(), new AccountUserValidator());
            _depoService = new DepoService(new DepoRepository(), new DepoValidation());
            _containerYardService = new ContainerYardService(new ContainerYardRepository(), new ContainerYardValidation());
            _employeeService = new EmployeeService(new EmployeeRepository(), new EmployeeValidation());
            _truckService = new TruckService(new TruckRepository(), new TruckValidation());
            _contactService = new ContactService(new ContactRepository(), new ContactValidation());
            _estimateProfitLossService = new EstimateProfitLossService(new EstimateProfitLossRepository(), new EstimateProfitLossValidation());
            _shipmentOrderDocumentService = new ShipmentOrderDocumentService(new ShipmentOrderDocumentRepository(), new ShipmentOrderDocumentValidation());
        }


        public ActionResult Index()
        {
            //if (!AuthenticationModel.IsAllowed("View", Core.Constants.Constant.MenuName.ShipmentOrder, Core.Constants.Constant.MenuGroupName.Master))
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
            var q = _shipmentOrderService.GetQueryable().Where(x => x.JobId == JobId && x.OfficeId == officeid); //.Include("ShipmentOrder").Include("UoM");

            var query = (from model in q
                         select new
                         {
                             model.IsDeleted,
                             model.Id,
                             ShipmentOrderId = model.ShipmentOrderCode,
                             model.LoadStatus,
                             model.ShipperName,
                             model.AgentName,
                             model.ConsigneeName,
                             model.ETD,
                             model.ETA,
                             Vessel = model.ShipmentOrderRoutings.FirstOrDefault() != null ? model.ShipmentOrderRoutings.FirstOrDefault().VesselName : "",
                             model.DeliveryPlaceName,
                             model.HouseBLNo,
                             model.SecondBLNo,
                             model.TotalSub,
                             model.JobClosed,
                             model.CreatedAt,
                             CreateBy = model.CreatedBy.Name,
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
                             model.ShipmentOrderId,
                             model.LoadStatus,
                             model.ShipperName,
                             model.AgentName,
                             model.ConsigneeName,
                             model.ETD,
                             model.ETA,
                             model.Vessel,
                             model.DeliveryPlaceName,
                             model.HouseBLNo,
                             model.SecondBLNo,
                             model.TotalSub,
                             model.JobClosed,
                             model.CreatedAt,
                             model.CreateBy,
                        }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }
         
        public dynamic GetListDocument(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "",
                                               int ShipmentOrderId = 0)
        {
            // Construct where statement
            int officeid = _accountUserService.GetObjectById(AuthenticationModel.GetUserId()).OfficeId;

            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _shipmentOrderDocumentService.GetQueryable().Where(x => x.ShipmentOrderId == ShipmentOrderId && x.IsDeleted == false);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.DocumentName,
                             model.SubmitDate,
                             model.Description,
                             model.CreatedAt,
                             CreatedBy = model.CreatedBy.Name
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
                           model.DocumentName,
                             model.Description,
                             model.SubmitDate,
                             model.CreatedAt,
                             model.CreatedBy
                        }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }


        public dynamic GetInfoDocument(int Id,string Name)
        {
            ShipmentOrderDocument model = new ShipmentOrderDocument();
            try
            {
                model = _shipmentOrderDocumentService.GetQueryable().Where(x => x.DocumentName == Name && x.ShipmentOrderId == Id && x.IsLegacy == true).FirstOrDefault();
                if (model == null)
                {
                    model = new ShipmentOrderDocument();
                    model.Errors = new Dictionary<string,string>();
                }

                return Json(new
                {
                      model.Id,
                      model.SubmitDate,
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
                    model.Errors
                }, JsonRequestBehavior.AllowGet);
            }
        }

        public dynamic GetLookUp(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "",
                                                int JobId = MasterConstant.JobType.PPJK)
        {
            // Construct where statement
            int officeid = _accountUserService.GetObjectById(AuthenticationModel.GetUserId()).OfficeId;

            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _shipmentOrderService.GetQueryable().Where(x => x.JobId == JobId && x.IsDeleted == false && x.OfficeId == officeid); //.Include("ShipmentOrder").Include("UoM");

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             ShipmentOrderId = model.ShipmentOrderCode,
                             model.AgentName,
                             model.ConsigneeName,
                             model.ETD,
                             model.CreatedAt
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
                             model.ShipmentOrderId,
                             model.ETD,
                             model.ConsigneeName,
                             model.AgentName,
                             model.CreatedAt
                        }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetLookUpInvoice(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "",
                                                int JobId = MasterConstant.JobType.PPJK)
        { 
            // Construct where statement
            int officeid = _accountUserService.GetObjectById(AuthenticationModel.GetUserId()).OfficeId;
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _estimateProfitLossService.GetQueryable().Where(x => x.ShipmentOrder.IsDeleted == false && x.IsDeleted == false && x.ShipmentOrder.JobId == JobId && x.ShipmentOrder.OfficeId == officeid); //.Include("ShipmentOrder").Include("UoM");

            var query = (from model in q
                         select new
                         {
                             model.ShipmentOrderId, 
                             ShipmentOrderCode = model.ShipmentOrder.ShipmentOrderCode,
                             model.ShipmentOrder.AgentName,
                             model.ShipmentOrder.ConsigneeName,
                             model.ShipmentOrder.ETD,
                             model.ShipmentOrder.CreatedAt
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
                        id = model.ShipmentOrderId,
                        cell = new object[] {
                             model.ShipmentOrderCode,
                             model.ETD,
                             model.ConsigneeName,
                             model.AgentName,
                             model.CreatedAt
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

            ShipmentOrder objJob = new ShipmentOrder();
            try
            {
               // objJob.CurrencyList = (from c in _countryService.GetCurrencyList() where c != "" orderby c select c).Distinct().ToList();
              //  objJob.JobOwnerList = (from j in _jobownerService.GetJobOwner("", "", "") select j).Distinct().ToList();
              //  objJob.VesselTypeList = _shipmentOrderService.GetVesselTypeList();

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
            ShipmentOrder model = new ShipmentOrder();
            try
            {
                model = _shipmentOrderService.GetObjectById(Id);
                return Json(new
                {
                             model.Id,
                             model.JobClosed,
                             model.ShipmentOrderCode,
                             model.JobNumber,
                             model.SubJobNumber,
                             model.TotalSub,
                             model.SIReference,
                             model.SIDate,
                             model.Conversion,
                             model.LoadStatus,
                             model.JobStatus,
                             model.ServiceNoID,
                             model.AgentId,
                             AgentCode = model.Agent != null ? model.Agent.MasterCode.ToString() : "",
                             model.AgentName,
                             model.AgentAddress,
                             TranshipmentId = model.TranshipmentId != null ? model.TranshipmentId.Value.ToString() : "",
                             model.TranshipmentName,
                             TranshipmentCode = model.Transhipment != null ? model.Transhipment.MasterCode.ToString() : "",
                             model.TranshipmentAddress,
                             ShipperId = model.ShipperId != null ? model.ShipperId.Value.ToString() : "",
                             ShipperCode = model.Shipper != null ? model.Shipper.MasterCode.ToString() : "",
                             model.ShipperName,
                             model.ShipperAddress,
                             ConsigneeId = model.ConsigneeId != null ? model.ConsigneeId.Value.ToString() : "",
                             ConsigneeCode = model.Consignee != null ? model.Consignee.MasterCode.ToString() : "",
                             model.ConsigneeName,
                             model.ConsigneeAddress,
                             NPartyId = model.NPartyId != null ? model.NPartyId.Value.ToString() : "",
                             model.NPartyName,
                             model.NPartyAddress,
                             model.GoodRecDate,
                             model.ETA,
                             model.ETD,
                             model.TA,
                             LoadingPortId = model.LoadingPortId != null ? model.LoadingPortId.Value.ToString() : "",
                             LoadingPortName = model.LoadingPort != null ? model.LoadingPortName : "",
                             LoadingIntPort =  model.LoadingPort != null ? model.LoadingPort.Abbrevation : "",
                             model.ReceiptPlaceId,
                             ReceiptPlaceName = model.ReceiptPlace != null ? model.ReceiptPlace.Name : "",
                             ReceiptPlaceIntName = model.ReceiptPlace != null ? model.ReceiptPlace.Abbrevation : "",
                             DischargePortId = model.DischargePortId != null ? model.DischargePortId.Value.ToString() : "",
                             DischargePortName = model.DischargePort != null ? model.DischargePortName : "",
                             DischargeIntPort = model.DischargePort != null ? model.DischargePort.Abbrevation : "",
                             DeliveryPlaceId = model.DeliveryPlaceId != null ? model.DeliveryPlaceId.Value.ToString() : "",
                             DeliveryPlaceIntCity = model.DeliveryPlace != null ? model.DeliveryPlace.Abbrevation : "",
                             DeliveryPlaceName = model.DeliveryPlace != null ? model.DeliveryPlaceName : "",
                             GoodDescription = model.GoodDescription != null ? model.GoodDescription : "",
                             model.HouseBLNo,
                             model.SecondBLNo,
                             model.WareHouseName,
                             model.KINS,
                             model.CFName,
                             model.OceanMSTBLNo,
                             model.VolumeBL,
                             model.VolumeInvoice, 
                             SSLineId = model.SSLineId != null ? model.SSLineId.Value.ToString() : "",
                             SSLineCode = model.SSLine != null ? model.SSLine.MasterCode.ToString() : "",
                             SSLineName = model.SSLine != null ? model.SSLine.ContactName : "",
                             EMKLId = model.EMKLId != null ? model.EMKLId.Value.ToString() : "",
                             EMKLCode = model.EMKL != null ? model.EMKL.MasterCode.ToString() : "",
                             EMKLName = model.EMKL != null ? model.EMKL.ContactName : "",
                             DepoId = model.DepoId != null ? model.DepoId.Value.ToString() : "",
                             DepoCode = model.Depo != null ? model.Depo.MasterCode.ToString() : "",
                             DepoName = model.Depo != null ? model.Depo.ContactName : "",
                             //PPJK
                             model.JobOrderPTP,
                             model.JobOrderCustomer,
                             model.InvoiceNo,
                             //----
                             ShipmentOrderRoutings = (from detail in model.ShipmentOrderRoutings
                                                      where detail.IsDeleted == false
                                     select new
                                     {  
                                          detail.Id,
                                          detail.VesselId,
                                          detail.VesselName,
                                          detail.VesselType,
                                          detail.Voyage,
                                          detail.ETD,
                                          CityId = detail.CityId != null ? detail.CityId.Value.ToString() : "",
                                          IntCity = detail.City != null ? detail.City.Abbrevation : "",
                                          CityName = detail.City != null ? detail.City.Name : "",
                                     }).ToArray(),
                             SeaContainerList = (from detail in model.SeaContainers
                                                 where detail.IsDeleted == false
                                                 select new
                                                      { 
                                                          detail.Id,
                                                          ContainerNo = detail.ContainerNo ?? "",
                                                          SealNo = detail.SealNo ?? "",
                                                          Size = detail.Size ?? 0,
                                                          Type = detail.Type ?? 0,
                                                          GrossWeight = detail.GrossWeight ?? 0,
                                                          NetWeight = detail.NetWeight ?? 0,
                                                          CBM = detail.CBM ?? 0, 
                                                          Commodity = detail.Commodity ?? "",
                                                          NoOfPieces = detail.NoOfPieces ?? 0,
                                                          PackagingCode = detail.PackagingCode ?? "",
                                                          PartOf = detail.PartOf ?? false,
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
                   model.Errors
                }, JsonRequestBehavior.AllowGet);
            }


        }

        [HttpPost]
        public dynamic Insert(ShipmentOrder model)
        {
            ////try
            {
                //if (!AuthenticationModel.IsAllowed("Create", Core.Constants.Constant.MenuName.ShipmentOrder, Core.Constants.Constant.MenuGroupName.Master))
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
                model = _shipmentOrderService.CreateObject(model,_officeService);

                return Json(new
                {
                    model.Errors,
                    shipmentId = model.Id,
                    jobNumber = model.JobNumber,
                    subJobNumber = model.SubJobNumber
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
        public dynamic InsertDocument(ShipmentOrderDocument model)
        { 
            ////try
            {
                //if (!AuthenticationModel.IsAllowed("Create", Core.Constants.Constant.MenuName.ShipmentOrder, Core.Constants.Constant.MenuGroupName.Master))
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
                model = _shipmentOrderDocumentService.CreateObject(model);

                return Json(new
                {
                    model.Errors,
                    shipmentId = model.Id,
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
        public dynamic UpdateDocument(ShipmentOrderDocument model)
        {
            ////try
            {
                //if (!AuthenticationModel.IsAllowed("Create", Core.Constants.Constant.MenuName.ShipmentOrder, Core.Constants.Constant.MenuGroupName.Master))
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
                var data = _shipmentOrderDocumentService.GetObjectById(model.Id);
                data.DocumentName = model.DocumentName;
                data.Description = model.Description;
                data.SubmitDate = model.SubmitDate;
                model = _shipmentOrderDocumentService.UpdateObject(data);

                return Json(new
                {
                    model.Errors,
                    shipmentId = model.Id,
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
        public dynamic DeleteDocument(ShipmentOrderDocument model)
        {
            ////try 
            {
                //if (!AuthenticationModel.IsAllowed("Create", Core.Constants.Constant.MenuName.ShipmentOrder, Core.Constants.Constant.MenuGroupName.Master))
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
                var data = _shipmentOrderDocumentService.GetObjectById(model.Id);
                model = _shipmentOrderDocumentService.SoftDeleteObject(data);

                return Json(new
                {
                    model.Errors,
                    shipmentId = model.Id,
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
        public dynamic Update(ShipmentOrder model)
        {
            try
            {
                //if (!AuthenticationModel.IsAllowed("Edit", Core.Constants.Constant.MenuName.ShipmentOrder, Core.Constants.Constant.MenuGroupName.Master))
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
                var data = _shipmentOrderService.GetObjectById(model.Id);
                data.JobNumber = model.JobNumber;
                data.SubJobNumber = model.SubJobNumber;
                data.LoadStatus = model.LoadStatus;
                data.ShipmentStatus = model.ShipmentStatus;
                data.JobStatus = model.JobStatus;
                data.SIReference = model.SIReference;
                data.SIDate = model.SIDate;
                data.AgentId = model.AgentId;
                data.AgentName = model.AgentName;
                data.AgentAddress = model.AgentAddress;
                data.ShipperId = model.ShipperId;
                data.ShipperName = model.ShipperName;
                data.ShipperAddress = model.ShipperAddress;
                data.ConsigneeId = model.ConsigneeId;
                data.ConsigneeName = model.ConsigneeName;
                data.ConsigneeAddress = model.ConsigneeAddress;
                data.LoadingPortId = model.LoadingPortId;
                data.LoadingPortName = model.LoadingPortName;
                data.ReceiptPlaceId = model.ReceiptPlaceId;
                data.ReceiptPlaceName = model.ReceiptPlaceName;
                data.DischargePortId = model.DischargePortId;
                data.DischargePortName = model.DischargePortName;
                data.DeliveryPlaceId = model.DeliveryPlaceId;
                data.DeliveryPlaceName = model.DeliveryPlaceName;
                data.ETD = model.ETD;
                data.ETA = model.ETA;
                data.TA = model.TA;
                data.GoodDescription = model.GoodDescription;
                data.HouseBLNo = model.HouseBLNo;
                data.SecondBLNo = model.SecondBLNo;
                data.JobOrderPTP = model.JobOrderPTP;
                data.JobOrderCustomer = model.JobOrderCustomer;
                data.InvoiceNo = model.InvoiceNo;
                data.SSLineId = model.SSLineId;
                data.EMKLId = model.EMKLId;
                data.DepoId = model.DepoId;
                data.WareHouseName = model.WareHouseName;
                data.KINS = model.KINS;
                data.CFName = model.CFName;
                model = _shipmentOrderService.UpdateObject(data);
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
        public dynamic Delete(ShipmentOrder model)
        {
            try
            {
                //if (!AuthenticationModel.IsAllowed("Delete", Core.Constants.Constant.MenuName.ShipmentOrder, Core.Constants.Constant.MenuGroupName.Master))
                //{
                //    Dictionary<string, string> Errors = new Dictionary<string, string>();
                //    Errors.Add("Generic", "You are Not Allowed to Delete Record");

                //    return Json(new
                //    {
                //        Errors
                //    }, JsonRequestBehavior.AllowGet);
                //}

                var data = _shipmentOrderService.GetObjectById(model.Id);
                int userId = AuthenticationModel.GetUserId();
                data.CreatedById = userId;
                data.OfficeId = _accountUserService.GetObjectById(userId).OfficeId;
                model = _shipmentOrderService.SoftDeleteObject(data);
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
        public dynamic Confirm(string job,int id,DateTime checkin)
        {
            ShipmentOrder model = new ShipmentOrder();
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
                var query = _shipmentOrderService.GetQueryable().Where(x => x.Id == Id && x.IsDeleted == false).ToList();
                if (query.FirstOrDefault().ShipmentOrderDocument != null)
                {
                    DateTime SPPBDate = (from data in query.FirstOrDefault().ShipmentOrderDocument
                                         where data.DocumentName == MasterConstant.Remarks.SPPB 
                                         select data.SubmitDate ?? DateTime.MinValue).FirstOrDefault().Date;
                    DateTime DOKORIDate = (from data in query.FirstOrDefault().ShipmentOrderDocument
                                         where data.DocumentName == MasterConstant.Remarks.DokOri
                                         select data.SubmitDate ?? DateTime.MinValue).FirstOrDefault().Date;
                    DateTime StrippingDate = (from data in query.FirstOrDefault().ShipmentOrderDocument
                                         where data.DocumentName == MasterConstant.Remarks.Stripping
                                         select data.SubmitDate ?? DateTime.MinValue).FirstOrDefault().Date;
                    DateTime SP2Date = (from data in query.FirstOrDefault().ShipmentOrderDocument
                                         where data.DocumentName == MasterConstant.Remarks.BuatSP2
                                         select data.SubmitDate ?? DateTime.MinValue).FirstOrDefault().Date;
                    DateTime H3Date = (from data in query.FirstOrDefault().ShipmentOrderDocument
                                         where data.DocumentName == MasterConstant.Remarks.H3
                                         select data.SubmitDate ?? DateTime.MinValue).FirstOrDefault().Date;
                    DateTime PBMDate = (from data in query.FirstOrDefault().ShipmentOrderDocument
                                         where data.DocumentName == MasterConstant.Remarks.PBM
                                         select data.SubmitDate ?? DateTime.MinValue).FirstOrDefault().Date;
                    DateTime MuatDate = (from data in query.FirstOrDefault().ShipmentOrderDocument
                                        where data.DocumentName == MasterConstant.Remarks.Muat
                                        select data.SubmitDate ?? DateTime.MinValue).FirstOrDefault().Date;
                    var listdata = (from data in query.FirstOrDefault().ShipmentOrderDocument
                                    select new
                                    {
                                        ConsigneeName = data.ShipmentOrder.ConsigneeName,
                                        BLAWBNo = data.ShipmentOrder.HouseBLNo,
                                        HBLHWBNo = data.ShipmentOrder.SecondBLNo,
                                        Shipper = data.ShipmentOrder.ShipperName,
                                        PortOfLoading = data.ShipmentOrder.LoadingPortName,
                                        AgentName = data.ShipmentOrder.AgentName,
                                        VesselName = data.ShipmentOrder.ShipmentOrderRoutings.FirstOrDefault().VesselName,
                                        JOPTP = data.ShipmentOrder.JobOrderPTP,
                                        JOCUST = data.ShipmentOrder.JobOrderCustomer,
                                        InvoiceNo = data.ShipmentOrder.InvoiceNo,
                                        ETA = data.ShipmentOrder.ETA ?? DateTime.MinValue,
                                        GoodDescription = data.ShipmentOrder.GoodDescription,
                                        SubmitDate = data.SubmitDate ?? DateTime.MinValue,
                                        DocumentName = data.DocumentName,
                                        SPPBDate = SPPBDate.Date,
                                        DOKORIDate = DOKORIDate.Date,
                                        StrippingDate = StrippingDate.Date,
                                        SP2Date = SP2Date.Date,
                                        H3Date = H3Date.Date,
                                        PBMDate = PBMDate.Date,
                                        MuatDate = MuatDate.Date,
                                    }).ToList();
                    rd.Load(Server.MapPath("~/") + "Reports/ShipmentOrder.rpt");

                    // Setting report data source
                    rd.SetDataSource(listdata);
                }
                else
                {
                    var listdata = (from data in query
                                    select new
                                    {
                                        ConsigneeName = data.ConsigneeName,
                                        BLAWBNo = data.HouseBLNo,
                                        HBLHWBNo = data.SecondBLNo,
                                        Shipper = data.ShipperName,
                                        PortOfLoading = data.LoadingPortName,
                                        AgentName = data.AgentName,
                                        VesselName = data.ShipmentOrderRoutings.FirstOrDefault().VesselName,
                                        JOPTP = data.JobOrderPTP,
                                        JOCUST = data.JobOrderCustomer,
                                        InvoiceNo = data.InvoiceNo,
                                        ETA = data.ETA,
                                        GoodDescription = data.GoodDescription,
                                        SubmitDate = DateTime.MinValue,
                                        DocumentName = "",
                                        SPPBDate = DateTime.MinValue,
                                        DOKORIDate = DateTime.MinValue,
                                        StrippingDate = DateTime.MinValue,
                                        SP2Date = DateTime.MinValue,
                                        H3Date = DateTime.MinValue,
                                        PBMDate = DateTime.MinValue,
                                        MuatDate = DateTime.MinValue,
                                    }).ToList();
                    rd.Load(Server.MapPath("~/") + "Reports/ShipmentOrder.rpt");

                    // Setting report data source
                    rd.SetDataSource(listdata);
                }
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
