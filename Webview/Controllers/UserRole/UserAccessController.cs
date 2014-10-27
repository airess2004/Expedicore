using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Service;
using Core.Interface.Service;
using Core.DomainModel;
using Data.Repository;
using Validation.Validation;
using System.Linq.Dynamic;
using System.Data.Entity;

namespace WebView.Controllers
{
    public class AccessUserController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("AccessUserController");
        private IAccountUserService _userAccountService;
        private IMenuUserService _userMenuService;
        private IAccessUserService _userAccessService;

        public AccessUserController()
        {
            _userAccountService = new AccountUserService(new AccountUserRepository(), new AccountUserValidator());
            _userAccessService = new AccessUserService(new AccessUserRepository(), new AccessUserValidator());
            _userMenuService = new MenuUserService(new MenuUserRepository(), new MenuUserValidator());
        }

        public dynamic GetAccessUser(int userId,string groupname)
        {
          
            try
            {
                int totalmenu = _userMenuService.GetQueryable().Count();
               var  q = _userAccessService.GetQueryableObjectsByAccountUserId(userId).Include("MenuUser");

               if (q.Count() < totalmenu)
               {
                   _userAccessService.CreateDefaultAccess(userId, _userMenuService, _userAccountService);
               }

               var model = (from m in q
                            select new
                            {
                                m.Id,
                                m.MenuUsers.Name,
                                m.MenuUsers.GroupName,
                                m.AllowView,
                                m.AllowCreate,
                                m.AllowEdit,
                                m.AllowDelete,
                                m.AllowUndelete,
                                m.AllowPrint,
                                m.AllowPaid,
                                m.AllowConfirm,
                                m.AllowReconcile,
                                m.AllowUnconfirm,
                                m.AllowUnpaid,
                                m.AllowUnreconcile
                                
                            }).ToList();
               return Json(new
              {
                  model
              }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                LOG.Error("GetAccessUserAccountingList", ex);
                Dictionary<string, string> Errors = new Dictionary<string, string>();
                Errors.Add("Generic", "Error " + ex);

                return Json(new
                {
                    Errors
                }, JsonRequestBehavior.AllowGet);
            }
         
           
        }

        [HttpPost]
        public dynamic Update(AccessUser model, string colName, bool isAllow)
        {
            try
            {
                if (!AuthenticationModel.IsAllowed("Edit", Core.Constants.Constant.MenuName.AccessUserRight, Core.Constants.Constant.MenuGroupName.Setting))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Edit record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                var data = _userAccessService.GetObjectById(model.Id);
                switch (colName)
                {
                    case "View": data.AllowView = isAllow; break;
                    case "Create": data.AllowCreate = isAllow; break;
                    case "Edit": data.AllowEdit = isAllow; break;
                    case "Delete": data.AllowDelete = isAllow; break;
                    case "UnDelete": data.AllowUndelete = isAllow; break;
                    case "Confirm": data.AllowConfirm = isAllow; break;
                    case "UnConfirm": data.AllowUnconfirm = isAllow; break;
                    case "Paid": data.AllowPaid = isAllow; break;
                    case "UnPaid": data.AllowUnpaid = isAllow; break;
                    case "Reconcile": data.AllowReconcile = isAllow; break;
                    case "UnReconcile": data.AllowUnreconcile = isAllow; break;
                    case "Print": data.AllowPrint = isAllow; break;
                }
                model = _userAccessService.UpdateObject(data, _userAccountService, _userMenuService);
            }
            catch (Exception ex)
            {
                LOG.Error("Update Failed", ex);
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


        public ActionResult Index()
        {
            if (!AuthenticationModel.IsAllowed("View", Core.Constants.Constant.MenuName.AccessUserRight, Core.Constants.Constant.MenuGroupName.Setting))
            {
                return Content(Core.Constants.Constant.ErrorPage.PageViewNotAllowed);
            }

            return View();
        }

    }
}
