using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;
using System.Web;
using System.Linq;
using Core.Interface.Service;
using Service;
using Data.Repository;
using Validation.Validation;
using Core.DomainModel;


namespace WebView
{
    public class AuthenticationModel
    {

        public static bool IsAuthenticated()
        {
            // IF IN MODE TESTING
            if (ConfigurationModels.MODE_TESTING())
                return true;


            if (HttpContext.Current.User != null)
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    return true;
                }
            }
            return false;
        }//end function IsAuthenticated

        public static bool IsAllowed(string Role, string MenuName, string MenuGroupName)
        {
            IAccountUserService _accountUserService = new AccountUserService(new AccountUserRepository(), new AccountUserValidator());
            IAccessUserService _accessUserService = new AccessUserService(new AccessUserRepository(), new AccessUserValidator());
            IMenuUserService _menuUserService = new MenuUserService(new MenuUserRepository(), new MenuUserValidator());

            AccountUser accountUser = _accountUserService.GetObjectById(GetUserId());
            if (accountUser == null) return false;
            if (accountUser.IsAdmin) return true;
            MenuUser menuUser = _menuUserService.GetObjectByNameAndGroupName(MenuName, MenuGroupName);
            if (menuUser != null)
            {
                AccessUser accessUser = _accessUserService.GetObjectByAccountUserIdAndMenuUserId(accountUser.Id, menuUser.Id);
                if (accessUser != null)
                {
                    switch (Role.ToLower())
                    {
                       
                        case "view": return accessUser.AllowView;
                        case "create": return accessUser.AllowCreate;
                        case "edit": return accessUser.AllowEdit;
                        case "delete": return accessUser.AllowDelete;
                        case "undelete": return accessUser.AllowUndelete;
                        case "confirm": return accessUser.AllowConfirm;
                        case "unconfirm": return accessUser.AllowUnconfirm;
                        case "paid": return accessUser.AllowPaid;
                        case "unpaid": return accessUser.AllowUnpaid;
                        case "reconcile": return accessUser.AllowReconcile;
                        case "unreconcile": return accessUser.AllowUnreconcile;
                        case "print": return accessUser.AllowPrint;
                    }
                }
            }
            return false;
        }//end function IsAllowed

        public static int GetUserId()
        {
            // IF IN MODE TESTING
            if (ConfigurationModels.MODE_TESTING())
                return ConfigurationModels.GET_MODE_TESTING_USERID();

            int userId = 0;
            if (HttpContext.Current.User != null)
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    if (HttpContext.Current.User.Identity is FormsIdentity)
                    {
                        // Get Forms Identity From Current User
                        FormsIdentity id = (FormsIdentity)
                        HttpContext.Current.User.Identity;

                        // Get Forms Ticket From Identity object
                        FormsAuthenticationTicket ticket = id.Ticket;

                        // Retrieve stored user-data (role information is assigned 
                        // when the ticket is created, separate multiple roles 
                        // with commas) 
                        string strUserId = ticket.Name;
                        userId = int.Parse(strUserId);
                    }
                }
            }
            return userId;
        }//end function GetUserCode




        public static string GetUserName()
        {
            // IF IN MODE TESTING
            if (ConfigurationModels.MODE_TESTING())
                return ConfigurationModels.GET_MODE_TESTING_USERNAME();

            string userName = "";
            if (HttpContext.Current.User != null)
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    if (HttpContext.Current.User.Identity is FormsIdentity)
                    {
                        // Get Forms Identity From Current User
                        FormsIdentity id = (FormsIdentity)
                        HttpContext.Current.User.Identity;

                        // Get Forms Ticket From Identity object
                        FormsAuthenticationTicket ticket = id.Ticket;

                        // Retrieve stored user-data (role information is assigned 
                        // when the ticket is created, separate multiple roles 
                        // with commas) 
                        string[] userDetail = ticket.UserData.Split('#');
                        if (!String.IsNullOrEmpty(userDetail[1]))
                            userName = userDetail[1];
                    }
                }
            }
            return userName;
        }//end function GetUserName
    }


    public class LoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

        // public List<SelectListItem> ListCompany { get; set; }
    }

    public class ChangePassword
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}