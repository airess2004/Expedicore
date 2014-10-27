using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Core.DomainModel;
using Core.Interface.Service;
using Data.Repository;
using Service;
using Validation.Validation;
using Core.Constants;

namespace Webview
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        private IMenuUserService _userMenuService;
        private IAccountUserService _userAccountService;
        private IAccessUserService _userAccessService;
        private IOfficeService _officeService;
        private Office office; 
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            PopulateData();
        }

        public void PopulateData()
        {
            _userMenuService = new MenuUserService(new MenuUserRepository(), new MenuUserValidator());
            _userAccountService = new AccountUserService(new AccountUserRepository(), new AccountUserValidator());
            _userAccessService = new AccessUserService(new AccessUserRepository(), new AccessUserValidator());
            _officeService = new OfficeService(new OfficeRepository(), new OfficeValidation());
            CreateSysAdmin();
        }

        public void CreateSysAdmin()
        {
            office = _officeService.GetQueryable().FirstOrDefault();
            if (office == null)
            {
                Office newOffice = new Office();
                newOffice.Name = "ExpedicoPPJK";

                office = _officeService.CreateObject(newOffice);
            }

            AccountUser userAccount = _userAccountService.GetObjectByUsername(Constant.UserType.Admin);
            if (userAccount == null)
            {
                userAccount = _userAccountService.CreateObject(Constant.UserType.Admin, "sysadmin", "Administrator", "Administrator", true,1);
            }

            //_userAccessService.CreateDefaultAccess(userAccount.Id, _userMenuService, _userAccountService);

        }
    }
}