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
    public class PortController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("PortController");
        private IPortService _portService;
        private IAccountUserService _accountUserService;
        private ICityLocationService _cityLocationService;
        public PortController()
        {
            _portService = new PortService(new PortRepository(), new PortValidation());
            _accountUserService = new AccountUserService(new AccountUserRepository(), new AccountUserValidator());
            _cityLocationService = new CityLocationService(new CityLocationRepository(), new CityLocationValidation());
        }


        public ActionResult Index()
        {
            //if (!AuthenticationModel.IsAllowed("View", Core.Constants.Constant.MenuName.Country, Core.Constants.Constant.MenuGroupName.Master))
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
            var q = _portService.GetQueryable().Where(x => x.OfficeId == officeid);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.IsDeleted,
                             model.MasterCode,
                             model.Abbrevation,
                             model.Name,
                             CityName = model.CityLocations.Name,
                             CountryName = model.CityLocations.CountryLocations.Name,
                             ContinentName = model.CityLocations.CountryLocations.Continents.Name,
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
                            model.Abbrevation,
                            model.Name,
                            model.CityName,
                            model.CountryName,
                            model.ContinentName,
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
            var q = _portService.GetQueryable().Where(x => x.OfficeId == officeid && x.IsDeleted == false);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.MasterCode,
                             model.Abbrevation,
                             model.Name,
                             CityName = model.CityLocations.Name,
                             CityAbbrevation = model.CityLocations.Abbrevation,
                             CountryName = model.CityLocations.CountryLocations.Name,
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
                            model.Name,
                            model.Abbrevation,
                            model.CityName,
                            model.CityAbbrevation,
                            model.CountryName,
                        }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }


        public dynamic GetInfo(int Id)
        {
            Port model = new Port();
            try
            {
                model = _portService.GetObjectById(Id);

                return Json(new
                {
                    model.Id,
                    model.MasterCode,
                    model.Name,
                    model.Abbrevation,
                    model.CityLocationId,
                    CityName = model.CityLocations.Name,
                    CityAbbrevation = model.CityLocations.Abbrevation,
                    CountryName = model.CityLocations.CountryLocations.Name,
                    CountryAbbrevation = model.CityLocations.CountryLocations.Abbrevation,
                    ContinentName = model.CityLocations.CountryLocations.Continents.Name,
                    ContinentAbbrevation = model.CityLocations.CountryLocations.Continents.Abbrevation,
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
        public dynamic Insert(Port model)
        {
            try
            {
                //if (!AuthenticationModel.IsAllowed("Create", Core.Constants.Constant.MenuName.Country, Core.Constants.Constant.MenuGroupName.Master))
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
                model = _portService.CreateObject(model,_cityLocationService);

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
        public dynamic Update(Port model)
        {
            try
            {
                //if (!AuthenticationModel.IsAllowed("Edit", Core.Constants.Constant.MenuName.Country, Core.Constants.Constant.MenuGroupName.Master))
                //{
                //    Dictionary<string, string> Errors = new Dictionary<string, string>();
                //    Errors.Add("Generic", "You are Not Allowed to Edit record");

                //    return Json(new
                //    {
                //        Errors
                //    }, JsonRequestBehavior.AllowGet);
                //}

                int userId = AuthenticationModel.GetUserId();
                model.UpdatedById = userId;
                model.OfficeId = _accountUserService.GetObjectById(userId).OfficeId;
                var data = _portService.GetObjectById(model.Id);
                data.Abbrevation = model.Abbrevation;
                data.Name = model.Name;
                model = _portService.UpdateObject(data,_cityLocationService);
            }
            catch (Exception ex)
            {
                LOG.Error("Update Failed", ex);
                Dictionary<string, string> Errors = new Dictionary<string, string>();
                Errors.Add("Generic", "Update Failed " + ex);

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
        public dynamic Delete(Port model)
        {
            try
            {
                //if (!AuthenticationModel.IsAllowed("Delete", Core.Constants.Constant.MenuName.Country, Core.Constants.Constant.MenuGroupName.Master))
                //{
                //    Dictionary<string, string> Errors = new Dictionary<string, string>();
                //    Errors.Add("Generic", "You are Not Allowed to Delete Record");

                //    return Json(new
                //    {
                //        Errors
                //    }, JsonRequestBehavior.AllowGet);
                //}
             
                var data = _portService.GetObjectById(model.Id);
                int userId = AuthenticationModel.GetUserId();
                data.UpdatedById = userId;
                data.OfficeId = _accountUserService.GetObjectById(userId).OfficeId;
                model = _portService.SoftDeleteObject(data);
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
