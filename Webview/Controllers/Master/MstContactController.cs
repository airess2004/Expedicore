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
    public class MstContactController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("ContactController");
        private IContactService _contactService;
        private IAccountUserService _accountUserService;
        private IEstimateProfitLossDetailService _estimateProfitLossDetailService;
        public MstContactController()
        {
            _contactService = new ContactService(new ContactRepository(), new ContactValidation());
            _accountUserService = new AccountUserService(new AccountUserRepository(), new AccountUserValidator());
            _estimateProfitLossDetailService = new EstimateProfitLossDetailService(new EstimateProfitLossDetailRepository(), new EstimateProfitLossDetailValidation());
        }


        public ActionResult Index()
        {
            //if (!AuthenticationModel.IsAllowed("View", Core.Constants.Constant.MenuName.Contact, Core.Constants.Constant.MenuGroupName.Master))
            //{
            //    return Content(Core.Constants.Constant.ErrorPage.PageViewNotAllowed);
            //}

            return View();
        }

        public dynamic GetList(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "")
        {
            // Construct where statement
            int officeid = _accountUserService.GetObjectById(AuthenticationModel.GetUserId()).OfficeId;
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _contactService.GetQueryable().Where(x => x.OfficeId == officeid);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.IsDeleted,
                             model.MasterCode,
                             model.ContactStatus,
                             model.ContactName,
                             ContactAs = model.IsAgent == true ? "Agent" 
                             : model.IsConsignee == true ? "Consignee" 
                             : model.IsDepo == true ? "Depo" 
                             : model.IsEMKL == true ? "EMKL"
                             : model.IsIATA == true ? "IATA"
                             : model.IsShipper == true ? "Shipper"
                             :"SSLine",
                             model.ContactPerson,
                             model.LastShipmentDate,
                             model.Phone,
                             model.Fax,
                             model.Email,
                             model.ContactAddress,
                             Port = model.Port != null ? model.Port.Name : "",
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
                            model.MasterCode,
                            model.ContactStatus,
                            model.ContactName,
                            model.ContactAs,
                            model.ContactPerson,
                            model.LastShipmentDate,
                            model.Phone,
                            model.Fax,
                            model.Email,
                            model.ContactAddress,
                            model.Port,
                            model.CreatedAt,
                            model.UpdatedAt,
                        }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }
         
        public dynamic GetLookUp(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "")
        {
            // Construct where statement
            int officeid = _accountUserService.GetObjectById(AuthenticationModel.GetUserId()).OfficeId;
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _contactService.GetQueryable().Where(x => x.OfficeId == officeid && x.IsDeleted == false);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.MasterCode,
                             model.ContactStatus,
                             model.ContactName,
                             ContactAs = model.IsAgent == true ? "Agent"
                             : model.IsConsignee == true ? "Consignee"
                             : model.IsDepo == true ? "Depo"
                             : model.IsEMKL == true ? "EMKL"
                             : model.IsIATA == true ? "IATA"
                             : model.IsShipper == true ? "Shipper"
                             : "SSLine",
                             CityCode = model.CityId,
                             IntCity = model.CityLocation != null ? model.CityLocation.Abbrevation : "",
                             CityName = model.CityLocation != null ? model.CityLocation.Name : "",
                             model.ContactAddress,
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
                            model.ContactStatus,
                            model.ContactName,
                            model.ContactAs,
                            model.ContactAddress,
                            model.CityCode,
                            model.IntCity,
                            model.CityName,
                        }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }
          
        public dynamic GetLookUpEPLIncome(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "",int ShipmentOrderId = 0, int amountCrr = MasterConstant.Currency.IDR)
        {
            // Construct where statement
            int officeid = _accountUserService.GetObjectById(AuthenticationModel.GetUserId()).OfficeId;
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _estimateProfitLossDetailService.GetQueryable().Where(x => x.EstimateProfitLoss.ShipmentOrderId == ShipmentOrderId && x.IsIncome == true && x.AmountCrr == amountCrr);

            var query = (from model in q
                         select new
                         {
                             model.ContactId,
                             model.Contact.ContactName,
                             model.Contact.ContactStatus,
                             ContactAs = model.Contact.IsAgent == true ? "Agent"
                            : model.Contact.IsConsignee == true ? "Consignee"
                            : model.Contact.IsDepo == true ? "Depo"
                            : model.Contact.IsEMKL == true ? "EMKL"
                            : model.Contact.IsIATA == true ? "IATA"
                            : model.Contact.IsShipper == true ? "Shipper"
                            : "SSLine",
                             model.Contact.MasterCode,
                             model.Contact.ContactAddress,
                         }).Where(filter).OrderBy(sidx + " " + sord).Distinct(); //.ToList();

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
                        id = model.ContactId,
                        cell = new object[] {
                            model.MasterCode,
                            model.ContactStatus,
                            model.ContactName,
                            model.ContactAs,
                            model.ContactAddress,
                        }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }
         
        public dynamic GetLookUpEPLCost(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "", int ShipmentOrderId = 0, int amountCrr = MasterConstant.Currency.IDR)
        {
            // Construct where statement
            int officeid = _accountUserService.GetObjectById(AuthenticationModel.GetUserId()).OfficeId;
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _estimateProfitLossDetailService.GetQueryable().Where(x => x.EstimateProfitLoss.ShipmentOrderId == ShipmentOrderId && x.IsIncome == false && x.AmountCrr == amountCrr);

            var query = (from model in q
                         select new
                         {
                             model.ContactId,
                             model.Contact.ContactName,
                             model.Contact.ContactStatus,
                             ContactAs = model.Contact.IsAgent == true ? "Agent"
                            : model.Contact.IsConsignee == true ? "Consignee"
                            : model.Contact.IsDepo == true ? "Depo"
                            : model.Contact.IsEMKL == true ? "EMKL"
                            : model.Contact.IsIATA == true ? "IATA"
                            : model.Contact.IsShipper == true ? "Shipper"
                            : "SSLine",
                             model.Contact.MasterCode,
                             model.Contact.ContactAddress,
                         }).Where(filter).OrderBy(sidx + " " + sord).Distinct(); //.ToList();

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
                        id = model.ContactId,
                        cell = new object[] {
                            model.MasterCode,
                            model.ContactStatus,
                            model.ContactName,
                            model.ContactAs,
                            model.ContactAddress,
                        }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }
        public dynamic GetLookUpAgent(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "")
        { 
            // Construct where statement
            int officeid = _accountUserService.GetObjectById(AuthenticationModel.GetUserId()).OfficeId;

            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _contactService.GetQueryable().Where(x => x.IsAgent == true && x.IsDeleted == false && x.OfficeId == officeid);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.MasterCode,
                             model.ContactName,
                             model.ContactAddress,
                             citycode = model.CityId,
                             intcity = model.CityLocation != null ? model.CityLocation.Abbrevation : "",
                             cityname = model.CityLocation != null ? model.CityLocation.Name : "",
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
                            model.ContactName,
                            model.ContactAddress,
                            model.citycode,
                            model.intcity,
                            model.cityname
                        }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetLookUpShipper(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "")
        {
            // Construct where statement 
            int officeid = _accountUserService.GetObjectById(AuthenticationModel.GetUserId()).OfficeId;

            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _contactService.GetQueryable().Where(x => x.IsShipper == true && x.IsDeleted == false && x.OfficeId == officeid);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.MasterCode,
                             model.ContactName,
                             model.ContactAddress,
                             citycode = model.CityId,
                             intcity = model.CityLocation != null ? model.CityLocation.Abbrevation : "",
                             cityname = model.CityLocation != null ? model.CityLocation.Name : "",
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
                            model.ContactName,
                            model.ContactAddress,
                            model.citycode,
                            model.intcity,
                            model.cityname
                        }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetLookUpConsignee(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "")
        { 
            // Construct where statement 
            int officeid = _accountUserService.GetObjectById(AuthenticationModel.GetUserId()).OfficeId;

            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _contactService.GetQueryable().Where(x => x.IsConsignee == true && x.IsDeleted == false && x.OfficeId == officeid); 

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.MasterCode,
                             model.ContactName,
                             model.ContactAddress,
                             citycode = model.CityId,
                             intcity = model.CityLocation != null ? model.CityLocation.Abbrevation : "",
                             cityname = model.CityLocation != null ? model.CityLocation.Name : "",
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
                            model.ContactName,
                            model.ContactAddress,
                            model.citycode,
                            model.intcity,
                            model.cityname
                        }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetLookUpSSLine(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "")
        { 
            // Construct where statement 
            int officeid = _accountUserService.GetObjectById(AuthenticationModel.GetUserId()).OfficeId;

            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _contactService.GetQueryable().Where(x => x.IsSSLine == true && x.IsDeleted == false && x.OfficeId == officeid);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.MasterCode,
                             model.ContactName,
                             model.ContactAddress,
                             citycode = model.CityId,
                             intcity = model.CityLocation != null ? model.CityLocation.Abbrevation : "",
                             cityname = model.CityLocation != null ? model.CityLocation.Name : "",
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
                            model.ContactName,
                            model.ContactAddress,
                            model.citycode,
                            model.intcity,
                            model.cityname
                        }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetLookUpDepo(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "")
        {
            // Construct where statement 
            int officeid = _accountUserService.GetObjectById(AuthenticationModel.GetUserId()).OfficeId;

            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _contactService.GetQueryable().Where(x => x.IsDepo == true && x.IsDeleted == false && x.OfficeId == officeid);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.MasterCode,
                             model.ContactName,
                             model.ContactAddress,
                             citycode = model.CityId,
                             intcity = model.CityLocation != null ? model.CityLocation.Abbrevation : "",
                             cityname = model.CityLocation != null ? model.CityLocation.Name : "",
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
                            model.ContactName,
                            model.ContactAddress,
                            model.citycode,
                            model.intcity,
                            model.cityname
                        }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetLookUpEMKL(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "")
        { 
            // Construct where statement 
            int officeid = _accountUserService.GetObjectById(AuthenticationModel.GetUserId()).OfficeId;

            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _contactService.GetQueryable().Where(x => x.IsEMKL == true && x.IsDeleted == false && x.OfficeId == officeid);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.MasterCode,
                             model.ContactName,
                             model.ContactAddress,
                             citycode = model.CityId,
                             intcity = model.CityLocation != null ? model.CityLocation.Abbrevation : "",
                             cityname = model.CityLocation != null ? model.CityLocation.Name : "",
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
                            model.ContactName,
                            model.ContactAddress,
                            model.citycode,
                            model.intcity,
                            model.cityname
                        }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetLookUpIATA(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "")
        {
            // Construct where statement  
            int officeid = _accountUserService.GetObjectById(AuthenticationModel.GetUserId()).OfficeId;

            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _contactService.GetQueryable().Where(x => x.IsIATA == true && x.IsDeleted == false && x.OfficeId == officeid);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.MasterCode,
                             model.ContactName,
                             model.ContactAddress,
                             citycode = model.CityId,
                             intcity = model.CityLocation != null ? model.CityLocation.Abbrevation : "",
                             cityname = model.CityLocation != null ? model.CityLocation.Name : "",
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
                            model.ContactName,
                            model.ContactAddress, 
                            model.citycode,
                            model.intcity,
                            model.cityname
                        }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfo(int Id)
        {
            Contact model = new Contact();
            try
            {
                model = _contactService.GetObjectById(Id);

                return Json(new
                {
                    model.Id,
                    model.IsDeleted,
                    model.MasterCode,
                    model.ContactStatus,
                    model.ContactName,
                    model.ContactPerson,
                    model.LastShipmentDate,
                    model.PostalCode,
                    model.Phone,
                    model.Fax,
                    model.Email,
                    model.ContactAddress,
                    model.PortId,
                    PortName =  model.PortId != null ? model.Port.Name : "",
                    PortAbbrevation = model.PortId != null ? model.Port.Abbrevation : "",
                    model.AirportId,
                    AirportName = model.AirportId != null ?  model.Airport.Name : "",
                    AirportAbbrevation = model.AirportId != null ? model.Airport.Abbrevation : "",
                    model.CityId,
                    CityAbbrevation = model.CityId != null ? model.CityLocation.Abbrevation : "",
                    CityName = model.CityId != null ? model.CityLocation.Name : "",
                    CountryName = model.CityId != null ? model.CityLocation.CountryLocations.Name : "",
                    ContinentName = model.CityId != null ? model.CityLocation.CountryLocations.Continents.Name : "",
                    model.NPPKP,   
                    model.NPWP,
                    model.IsAgent,
                    model.IsConsignee,
                    model.IsEMKL,
                    model.IsIATA,
                    model.IsShipper,
                    model.IsSSLine,
                    model.IsDepo,
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
                   model.Errors
                }, JsonRequestBehavior.AllowGet);
            }


        }

        [HttpPost]
        public dynamic Insert(Contact model)
        {
            try
            {
                //if (!AuthenticationModel.IsAllowed("Create", Core.Constants.Constant.MenuName.Contact, Core.Constants.Constant.MenuGroupName.Master))
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
                model = _contactService.CreateObject(model);

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
        public dynamic Update(Contact model)
        {
            try
            {
                //if (!AuthenticationModel.IsAllowed("Edit", Core.Constants.Constant.MenuName.Contact, Core.Constants.Constant.MenuGroupName.Master))
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
                var data = _contactService.GetObjectById(model.Id);
                data.ContactName = model.ContactName;
                data.ContactStatus = model.ContactStatus;
                data.ContactAddress = model.ContactAddress;
                data.ContactPerson = model.ContactPerson;
                data.PostalCode = model.PostalCode;
                data.Phone = model.Phone;
                data.NPWP = model.NPWP;
                data.NPPKP = model.NPPKP;
                data.Fax = model.Fax;
                data.Email = model.Email;
                data.CityId = model.CityId;
                data.PortId = model.PortId;
                data.AirportId = model.AirportId;
                data.IsAgent = model.IsAgent;
                data.IsShipper = model.IsShipper;
                data.IsDepo = model.IsDepo;
                data.IsConsignee = model.IsConsignee;
                data.ContactAddress = model.ContactAddress;
                model = _contactService.UpdateObject(data);
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
        public dynamic Delete(Contact model)
        {
            try
            {
                //if (!AuthenticationModel.IsAllowed("Delete", Core.Constants.Constant.MenuName.Contact, Core.Constants.Constant.MenuGroupName.Master))
                //{
                //    Dictionary<string, string> Errors = new Dictionary<string, string>();
                //    Errors.Add("Generic", "You are Not Allowed to Delete Record");

                //    return Json(new
                //    {
                //        Errors
                //    }, JsonRequestBehavior.AllowGet);
                //}

                var data = _contactService.GetObjectById(model.Id);
                int userId = AuthenticationModel.GetUserId();
                model = _contactService.SoftDeleteObject(data);
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
    }
}
