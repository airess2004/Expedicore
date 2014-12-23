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
    public class MstContactController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("ContactController");
        private IContactService _contactService;
        private IAccountUserService _accountUserService;
        public MstContactController()
        {
            _contactService = new ContactService(new ContactRepository(), new ContactValidation());
            _accountUserService = new AccountUserService(new AccountUserRepository(), new AccountUserValidator());
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

            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _contactService.GetQueryable(); //.Include("Contact").Include("UoM");

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
                             model.CreatedAt,
                             model.UpdatedAt,
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
                data.CreatedById = userId;
                data.OfficeId = _accountUserService.GetObjectById(userId).OfficeId;
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
