﻿using System;
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
    public class InvoiceController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("InvoiceController");
        private IInvoiceService _invoiceService;
        private IExchangeRateService _exchangeRateService;
        private IInvoiceDetailService _invoiceDetailService;
        private IShipmentOrderService _shipmentOrderService;
        private IAccountUserService _accountUserService;
        private IDepoService _depoService;
        private IContainerYardService _containerYardService;
        private IEmployeeService _employeeService;
        private ITruckService _truckService;
        private IContactService _contactService;
        private IOfficeService _officeService;
        
        public InvoiceController()
        {
            _invoiceService = new InvoiceService(new InvoiceRepository(), new InvoiceValidation());
            _invoiceDetailService = new InvoiceDetailService(new InvoiceDetailRepository(), new InvoiceDetailValidation());
            _shipmentOrderService = new ShipmentOrderService(new ShipmentOrderRepository(), new ShipmentOrderValidation());
            _officeService = new OfficeService(new OfficeRepository(),new OfficeValidation());
            _accountUserService = new AccountUserService(new AccountUserRepository(), new AccountUserValidator());
            _depoService = new DepoService(new DepoRepository(), new DepoValidation());
            _containerYardService = new ContainerYardService(new ContainerYardRepository(), new ContainerYardValidation());
            _employeeService = new EmployeeService(new EmployeeRepository(), new EmployeeValidation());
            _truckService = new TruckService(new TruckRepository(), new TruckValidation());
            _contactService = new ContactService(new ContactRepository(), new ContactValidation());
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
            var q = _invoiceService.GetQueryable().Where(x => x.OfficeId == officeid); //.Include("Invoice").Include("UoM");

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.IsDeleted,
                             model.CreatedAt,
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
                          model.CreatedAt,
                          model.UpdatedAt,
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

            Invoice objJob = new Invoice();
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
                data.CreatedById = userId;
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
        public dynamic Confirm(string job,int id,DateTime checkin)
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

    }
}