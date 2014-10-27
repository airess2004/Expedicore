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

namespace WebView.Controllers
{
    public class TruckOrderController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("TruckOrderController");
        private ITruckOrderService _truckOrderService;
        private IAccountUserService _accountUserService;
        private IDepoService _depoService;
        private IContainerYardService _containerYardService;
        private IEmployeeService _employeeService;
        private ITruckService _truckService;
        private IContactService _contactService;

        public TruckOrderController()
        {
            _truckOrderService = new TruckOrderService(new TruckOrderRepository(), new TruckOrderValidation());
            _accountUserService = new AccountUserService(new AccountUserRepository(), new AccountUserValidator());
            _depoService = new DepoService(new DepoRepository(), new DepoValidation());
            _containerYardService = new ContainerYardService(new ContainerYardRepository(), new ContainerYardValidation());
            _employeeService = new EmployeeService(new EmployeeRepository(), new EmployeeValidation());
            _truckService = new TruckService(new TruckRepository(), new TruckValidation());
            _contactService = new ContactService(new ContactRepository(), new ContactValidation());
        }


        public ActionResult Index()
        {
            //if (!AuthenticationModel.IsAllowed("View", Core.Constants.Constant.MenuName.TruckOrder, Core.Constants.Constant.MenuGroupName.Master))
            //{
            //    return Content(Core.Constants.Constant.ErrorPage.PageViewNotAllowed);
            //}

            return View();
        }

        public dynamic GetList(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "")
        {
            // Construct where statement

            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _truckOrderService.GetQueryable().Include("Truck:").Include("Depo").Include("Employee").Include("ContainerYard").Include("Contact"); //.Include("TruckOrder").Include("UoM");

            var query = (from model in q
                         select new
                         {
                             model.IsFinished,
                             model.Id,
                             model.NoJob,
                             model.TanggalSPPB,
                             model.TanggalOrder,
                             model.AgendaTruck,
                             model.PICOrder,
                             Contact = model.Contact.ContactName,

                             model.NoContainer,
                             model.Party,
                             model.InvoiceNo,
                             model.InvoiceShipper,
                             Truck = model.Truck.NoPlat,
                             Employee = model.Employee.Name,
                             
                             model.TglLoloBayar,
                             model.PIC,
                             model.AgendaMTP,
                             model.Tujuan,
                             ContainerYard = model.ContainerYard.Name,
                             Depo = model.Depo.Name,
                             model.UJ,
                             model.CYIN,
                             model.CYOUT,
                             model.MILLIN,
                             model.REGIN,
                             model.REGOUT,
                             model.MILLOUT,
                             model.BOGASARI,
                             model.DEPOIN,
                             model.DEPOOUT,
                             model.GARASI,
                             model.CYMILL,
                             model.DIMILL,
                             model.OTWKEPRIOK,
                             model.ADATOLCIUJUNG,
                             model.JORR,
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
                          model.IsFinished,
                          model.Id,
                             model.NoJob,
                             model.TanggalSPPB,
                             model.TanggalOrder,
                             model.AgendaTruck,
                             model.PICOrder,
                             model.Contact,
                             model.AgendaMTP,
                             model.InvoiceShipper,
                             model.Party,
                             model.PIC,
                             model.Truck,
                             model.Employee,
                             model.NoContainer,
                             model.ContainerYard,
                             model.Tujuan,
                             model.Depo,
                             model.TglLoloBayar,
                             model.InvoiceNo,
                             model.UJ,
                             model.CYIN,
                             model.CYOUT,
                             model.MILLIN,
                             model.REGIN,
                             model.REGOUT,
                             model.MILLOUT,
                             model.BOGASARI,
                             model.DEPOIN,
                             model.DEPOOUT,
                             model.GARASI,
                             model.CYMILL,
                             model.DIMILL,
                             model.OTWKEPRIOK,
                             model.ADATOLCIUJUNG,
                             model.JORR,
                             model.CreatedAt,
                             model.UpdatedAt,
                        }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetListNOTMILLIN(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "")
        {
            // Construct where statement

            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _truckOrderService.GetQueryable().Include("Truck:").Include("Depo").Include("Employee").Include("ContainerYard").Include("Contact").Where(x=>x.CYIN != null && x.MILLIN == null); //.Include("TruckOrder").Include("UoM");

            var query = (from model in q
                         select new
                         {
                             model.IsFinished,
                             model.Id,
                             model.NoJob,
                             model.TanggalSPPB,
                             model.TanggalOrder,
                             model.AgendaTruck,
                             model.PICOrder,
                             Contact = model.Contact.ContactName,

                             model.NoContainer,
                             model.Party,
                             model.InvoiceNo,
                             model.InvoiceShipper,
                             Truck = model.Truck.NoPlat,
                             Employee = model.Employee.Name,

                             model.TglLoloBayar,
                             model.PIC,
                             model.AgendaMTP,
                             model.Tujuan,
                             ContainerYard = model.ContainerYard.Name,
                             Depo = model.Depo.Name,
                             model.UJ,
                             model.CYIN,
                             model.CYOUT,
                             model.MILLIN,
                             model.REGIN,
                             model.REGOUT,
                             model.MILLOUT,
                             model.BOGASARI,
                             model.DEPOIN,
                             model.DEPOOUT,
                             model.GARASI,
                             model.CYMILL,
                             model.DIMILL,
                             model.OTWKEPRIOK,
                             model.ADATOLCIUJUNG,
                             model.JORR,
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
                          model.IsFinished,
                          model.Id,
                             model.NoJob,
                             model.TanggalSPPB,
                             model.TanggalOrder,
                             model.AgendaTruck,
                             model.PICOrder,
                             model.Contact,
                             model.AgendaMTP,
                             model.InvoiceShipper,
                             model.Party,
                             model.PIC,
                             model.Truck,
                             model.Employee,
                             model.NoContainer,
                             model.ContainerYard,
                             model.Tujuan,
                             model.Depo,
                             model.TglLoloBayar,
                             model.InvoiceNo,
                             model.UJ,
                             model.CYIN,
                             model.CYOUT,
                             model.MILLIN,
                             model.REGIN,
                             model.REGOUT,
                             model.MILLOUT,
                             model.BOGASARI,
                             model.DEPOIN,
                             model.DEPOOUT,
                             model.GARASI,
                             model.CYMILL,
                             model.DIMILL,
                             model.OTWKEPRIOK,
                             model.ADATOLCIUJUNG,
                             model.JORR,
                             model.CreatedAt,
                             model.UpdatedAt,
                        }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }
        public dynamic GetListNOTCYIN(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "")
        { 
            // Construct where statement

            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _truckOrderService.GetQueryable().Include("Truck:").Include("Depo").Include("Employee").Include("ContainerYard").Include("Contact").Where(x => x.CYIN == null); //.Include("TruckOrder").Include("UoM");

            var query = (from model in q
                         select new
                         {
                             model.IsFinished,
                             model.Id,
                             model.NoJob,
                             model.TanggalSPPB,
                             model.TanggalOrder,
                             model.AgendaTruck,
                             model.PICOrder,
                             Contact = model.Contact.ContactName,

                             model.NoContainer,
                             model.Party,
                             model.InvoiceNo,
                             model.InvoiceShipper,
                             Truck = model.Truck.NoPlat,
                             Employee = model.Employee.Name,

                             model.TglLoloBayar,
                             model.PIC,
                             model.AgendaMTP,
                             model.Tujuan,
                             ContainerYard = model.ContainerYard.Name,
                             Depo = model.Depo.Name,
                             model.UJ,
                             model.CYIN,
                             model.CYOUT,
                             model.MILLIN,
                             model.REGIN,
                             model.REGOUT,
                             model.MILLOUT,
                             model.BOGASARI,
                             model.DEPOIN,
                             model.DEPOOUT,
                             model.GARASI,
                             model.CYMILL,
                             model.DIMILL,
                             model.OTWKEPRIOK,
                             model.ADATOLCIUJUNG,
                             model.JORR,
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
                          model.IsFinished,
                          model.Id,
                             model.NoJob,
                             model.TanggalSPPB,
                             model.TanggalOrder,
                             model.AgendaTruck,
                             model.PICOrder,
                             model.Contact,
                             model.AgendaMTP,
                             model.InvoiceShipper,
                             model.Party,
                             model.PIC,
                             model.Truck,
                             model.Employee,
                             model.NoContainer,
                             model.ContainerYard,
                             model.Tujuan,
                             model.Depo,
                             model.TglLoloBayar,
                             model.InvoiceNo,
                             model.UJ,
                             model.CYIN,
                             model.CYOUT,
                             model.MILLIN,
                             model.REGIN,
                             model.REGOUT,
                             model.MILLOUT,
                             model.BOGASARI,
                             model.DEPOIN,
                             model.DEPOOUT,
                             model.GARASI,
                             model.CYMILL,
                             model.DIMILL,
                             model.OTWKEPRIOK,
                             model.ADATOLCIUJUNG,
                             model.JORR,
                             model.CreatedAt,
                             model.UpdatedAt,
                        }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }
        public dynamic GetListNOTDEPOIN(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "")
        {
            // Construct where statement

            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _truckOrderService.GetQueryable().Include("Truck:").Include("Depo").Include("Employee").Include("ContainerYard").Include("Contact").Where(x => x.CYIN != null && x.MILLIN != null && x.DEPOIN == null); //.Include("TruckOrder").Include("UoM");

            var query = (from model in q
                         select new
                         {
                             model.IsFinished,
                             model.Id,
                             model.NoJob,
                             model.TanggalSPPB,
                             model.TanggalOrder,
                             model.AgendaTruck,
                             model.PICOrder,
                             Contact = model.Contact.ContactName,

                             model.NoContainer,
                             model.Party,
                             model.InvoiceNo,
                             model.InvoiceShipper,
                             Truck = model.Truck.NoPlat,
                             Employee = model.Employee.Name,

                             model.TglLoloBayar,
                             model.PIC,
                             model.AgendaMTP,
                             model.Tujuan,
                             ContainerYard = model.ContainerYard.Name,
                             Depo = model.Depo.Name,
                             model.UJ,
                             model.CYIN,
                             model.CYOUT,
                             model.MILLIN,
                             model.REGIN,
                             model.REGOUT,
                             model.MILLOUT,
                             model.BOGASARI,
                             model.DEPOIN,
                             model.DEPOOUT,
                             model.GARASI,
                             model.CYMILL,
                             model.DIMILL,
                             model.OTWKEPRIOK,
                             model.ADATOLCIUJUNG,
                             model.JORR,
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
                          model.IsFinished,
                          model.Id,
                             model.NoJob,
                             model.TanggalSPPB,
                             model.TanggalOrder,
                             model.AgendaTruck,
                             model.PICOrder,
                             model.Contact,
                             model.AgendaMTP,
                             model.InvoiceShipper,
                             model.Party,
                             model.PIC,
                             model.Truck,
                             model.Employee,
                             model.NoContainer,
                             model.ContainerYard,
                             model.Tujuan,
                             model.Depo,
                             model.TglLoloBayar,
                             model.InvoiceNo,
                             model.UJ,
                             model.CYIN,
                             model.CYOUT,
                             model.MILLIN,
                             model.REGIN,
                             model.REGOUT,
                             model.MILLOUT,
                             model.BOGASARI,
                             model.DEPOIN,
                             model.DEPOOUT,
                             model.GARASI,
                             model.CYMILL,
                             model.DIMILL,
                             model.OTWKEPRIOK,
                             model.ADATOLCIUJUNG,
                             model.JORR,
                             model.CreatedAt,
                             model.UpdatedAt,
                        }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }
        public dynamic GetListIsFinish(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "")
        {
            // Construct where statement

            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _truckOrderService.GetQueryable().Include("Truck:").Include("Depo").Include("Employee").Include("ContainerYard").Include("Contact").Where(x => x.CYIN != null && x.MILLIN != null && x.DEPOIN != null && x.GARASI != null); //.Include("TruckOrder").Include("UoM");

            var query = (from model in q
                         select new
                         {
                             model.IsFinished,
                             model.Id,
                             model.NoJob,
                             model.TanggalSPPB,
                             model.TanggalOrder,
                             model.AgendaTruck,
                             model.PICOrder,
                             Contact = model.Contact.ContactName,

                             model.NoContainer,
                             model.Party,
                             model.InvoiceNo,
                             model.InvoiceShipper,
                             Truck = model.Truck.NoPlat,
                             Employee = model.Employee.Name,

                             model.TglLoloBayar,
                             model.PIC,
                             model.AgendaMTP,
                             model.Tujuan,
                             ContainerYard = model.ContainerYard.Name,
                             Depo = model.Depo.Name,
                             model.UJ,
                             model.CYIN,
                             model.CYOUT,
                             model.MILLIN,
                             model.REGIN,
                             model.REGOUT,
                             model.MILLOUT,
                             model.BOGASARI,
                             model.DEPOIN,
                             model.DEPOOUT,
                             model.GARASI,
                             model.CYMILL,
                             model.DIMILL,
                             model.OTWKEPRIOK,
                             model.ADATOLCIUJUNG,
                             model.JORR,
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
                          model.IsFinished,
                          model.Id,
                             model.NoJob,
                             model.TanggalSPPB,
                             model.TanggalOrder,
                             model.AgendaTruck,
                             model.PICOrder,
                             model.Contact,
                             model.AgendaMTP,
                             model.InvoiceShipper,
                             model.Party,
                             model.PIC,
                             model.Truck,
                             model.Employee,
                             model.NoContainer,
                             model.ContainerYard,
                             model.Tujuan,
                             model.Depo,
                             model.TglLoloBayar,
                             model.InvoiceNo,
                             model.UJ,
                             model.CYIN,
                             model.CYOUT,
                             model.MILLIN,
                             model.REGIN,
                             model.REGOUT,
                             model.MILLOUT,
                             model.BOGASARI,
                             model.DEPOIN,
                             model.DEPOOUT,
                             model.GARASI,
                             model.CYMILL,
                             model.DIMILL,
                             model.OTWKEPRIOK,
                             model.ADATOLCIUJUNG,
                             model.JORR,
                             model.CreatedAt,
                             model.UpdatedAt,
                        }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }
        public dynamic GetInfo(int Id)
        {
            TruckOrder model = new TruckOrder();
            try
            {
                model = _truckOrderService.GetObjectById(Id);

                return Json(new
                {
                             model.Id,
                             model.NoJob,
                             model.NoContainer,
                             model.Party,
                             model.InvoiceNo,
                             model.InvoiceShipper,
                             model.ContactId,
                             Contact = _contactService.GetObjectById(model.ContactId).ContactName,
                             model.TruckId,
                             Truck = _truckService.GetObjectById(model.TruckId).NoPlat,
                             model.EmployeeId,
                             Employee = _employeeService.GetObjectById(model.EmployeeId).Name,
                             model.TanggalSPPB,
                             model.TanggalOrder,
                             model.TglLoloBayar,
                             model.PIC,
                             model.PICOrder,
                             model.AgendaMTP,
                             model.AgendaTruck,
                             model.ContainerYardId,
                             model.Tujuan,
                             ContainerYard = _containerYardService.GetObjectById(model.ContainerYardId).Name,
                             model.DepoId,
                             Depo = _depoService.GetObjectById(model.DepoId).Name,
                             model.UJ,
                             model.CYIN,
                             model.CYOUT,
                             model.MILLIN,
                             model.REGIN,
                             model.REGOUT,
                             model.MILLOUT,
                             model.BOGASARI,
                             model.DEPOIN,
                             model.DEPOOUT,
                             model.GARASI,
                             model.CYMILL,
                             model.DIMILL,
                             model.OTWKEPRIOK,
                             model.ADATOLCIUJUNG,
                             model.JORR,
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
        public dynamic Insert(TruckOrder model)
        {
            try
            {
                //if (!AuthenticationModel.IsAllowed("Create", Core.Constants.Constant.MenuName.TruckOrder, Core.Constants.Constant.MenuGroupName.Master))
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
                model.IsFinished = false;
                model = _truckOrderService.CreateObject(model);

                return Json(new
                {
                    model.Errors
                });
            }
            catch (Exception ex)
            {
                LOG.Error("Insert Failed", ex);
                Dictionary<string, string> Errors = new Dictionary<string, string>();
                Errors.Add("Generic", "Insert Failed " + ex);

                return Json(new
                {
                    model.Errors
                });
            }


        }

        [HttpPost]
        public dynamic Update(TruckOrder model)
        {
            try
            {
                //if (!AuthenticationModel.IsAllowed("Edit", Core.Constants.Constant.MenuName.TruckOrder, Core.Constants.Constant.MenuGroupName.Master))
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
                var data = _truckOrderService.GetObjectById(model.Id);
               
                         data.NoJob =  model.NoJob;
                         data.NoContainer = model.NoContainer;
                         data.Party =  model.Party;
                         data.InvoiceNo = model.InvoiceNo;
                         data.InvoiceShipper = model.InvoiceShipper;
                         data.InvoiceNo = model.InvoiceNo;
                         data.InvoiceShipper=  model.InvoiceShipper;
                          data.TruckId = model.TruckId;
                        data.EmployeeId = model.EmployeeId;
                        data.TanggalSPPB = model.TanggalSPPB;
                        data.TanggalOrder = model.TanggalOrder;
                        data.TglLoloBayar = model.TglLoloBayar;
                        data.PIC = model.PIC;
                        data.Tujuan = model.Tujuan;
                        data.PICOrder  = model.PICOrder;
                        data.AgendaMTP = model.AgendaMTP;
                        data.AgendaTruck = model.AgendaTruck;
                        data.ContainerYardId = model.ContainerYardId;
                        data.DepoId = model.DepoId;
                model = _truckOrderService.UpdateObject(data);
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
        public dynamic Delete(TruckOrder model)
        {
            try
            {
                //if (!AuthenticationModel.IsAllowed("Delete", Core.Constants.Constant.MenuName.TruckOrder, Core.Constants.Constant.MenuGroupName.Master))
                //{
                //    Dictionary<string, string> Errors = new Dictionary<string, string>();
                //    Errors.Add("Generic", "You are Not Allowed to Delete Record");

                //    return Json(new
                //    {
                //        Errors
                //    }, JsonRequestBehavior.AllowGet);
                //}

                var data = _truckOrderService.GetObjectById(model.Id);
                int userId = AuthenticationModel.GetUserId();
                data.CreatedById = userId;
                data.OfficeId = _accountUserService.GetObjectById(userId).OfficeId;
                model = _truckOrderService.SoftDeleteObject(data);
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
            TruckOrder model = new TruckOrder();
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
               
                var data = _truckOrderService.GetObjectById(id);
                  switch (job)
                  {
                      case "UJ":
                          data.UJ = checkin;
                          break;
                      case "CYIN":
                          data.CYIN = checkin;
                          break;
                      case "CYOUT":
                          data.CYOUT = checkin;
                          break;
                      case "MILLIN":
                          data.MILLIN = checkin;
                          break;
                      case "REGIN":
                          data.REGIN = checkin;
                          break;
                      case "REGOUT":
                          data.REGOUT = checkin;
                          break;
                      case "MILLOUT":
                          data.MILLOUT = checkin;
                          break;
                      case "BOGASARI":
                          data.BOGASARI = checkin;
                          break;
                      case "DEPOIN":
                          data.DEPOIN = checkin;
                          break;
                      case "DEPOOUT":
                          data.DEPOOUT = checkin;
                          break;
                      case "GARASI":
                          data.GARASI = checkin;
                          break;
                      case "CYMILL":
                          data.CYMILL = checkin;
                          break;
                      case "DIMILL":
                          data.DIMILL = checkin;
                          break;
                      case "OTWKEPRIOK":
                          data.OTWKEPRIOK = checkin;
                          break;
                      case "ADATOLCIUJUNG":
                          data.ADATOLCIUJUNG = checkin;
                          break;
                      case "JORR":
                          data.JORR = checkin;
                          break;
                  }
                  model.Errors = new Dictionary<string, string>();
                data = _truckOrderService.UpdateObject(data);
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
        public dynamic Finish(TruckOrder model)
        {
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
                model.Errors = new Dictionary<string, string>();
                var data = _truckOrderService.GetObjectById(model.Id);
                data.IsFinished = true;
                data = _truckOrderService.UpdateObject(data);
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
