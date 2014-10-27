using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Interface.Validation;


namespace Service
{
    public class AccountUserService : IAccountUserService
    {
        private IAccountUserRepository _repository;
        private IAccountUserValidator _validator;

        public AccountUserService(IAccountUserRepository _AccountUserRepository, IAccountUserValidator _AccountUserValidator)
        {
            _repository = _AccountUserRepository;
            _validator = _AccountUserValidator;
        }

        public IAccountUserValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<AccountUser> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<AccountUser> GetAll()
        {
            return _repository.GetAll();
        }

        public AccountUser GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public AccountUser GetObjectByIsAdmin(bool IsAdmin)
        {
            return _repository.GetObjectByIsAdmin(IsAdmin);
        }

        public AccountUser GetObjectByUsername(string username)
        {
            return _repository.GetObjectByUsername(username);
        }

        public AccountUser IsLoginValid(string username, string password)
        {
            StringEncryption se = new StringEncryption();
            return _repository.IsLoginValid(username, se.Encrypt(password));
        }

        public AccountUser CreateObject(AccountUser AccountUser)
        {
            AccountUser.Errors = new Dictionary<String, String>();
            if(_validator.ValidCreateObject(AccountUser, this))
            {
                AccountUser.Username = AccountUser.Username.Trim();
                StringEncryption se = new StringEncryption();
                //string realpassword = AccountUser.Password;
                AccountUser.Password = se.Encrypt(AccountUser.Password);
                _repository.CreateObject(AccountUser);
                //AccountUser.Password = realpassword; // set back to unencrypted password to prevent encrypting an already encrypted password on the next update
            }
            return AccountUser;
        }

        public AccountUser CreateObject(string username, string password, string name, string description, bool IsAdmin,int officeId)
        {
            AccountUser AccountUser = new AccountUser()
            {
                Username = username,
                Password = password,
                Name = name,
                Description = description,
                IsAdmin = IsAdmin,
                OfficeId = officeId
            };
            return CreateObject(AccountUser);
        }

        public AccountUser UpdateObjectPassword(AccountUser AccountUser, string OldPassword, string NewPassword, string ConfirmPassword)
        {
            if (_validator.ValidUpdateObjectPassword(AccountUser, OldPassword, NewPassword, ConfirmPassword, this))
            {
                AccountUser.Username = AccountUser.Username.Trim();
                StringEncryption se = new StringEncryption();
                //string realpassword = AccountUser.Password;
                AccountUser.Password = se.Encrypt(NewPassword);
                _repository.UpdateObject(AccountUser);
                //AccountUser.Password = realpassword; // set back to unencrypted password to prevent encrypting an already encrypted password on the next update
            }
            return AccountUser;
        }

        public AccountUser UpdateObject(AccountUser AccountUser)
        {
            if(_validator.ValidUpdateObject(AccountUser, this))
            {
                AccountUser.Username = AccountUser.Username.Trim();
                StringEncryption se = new StringEncryption();
                //string realpassword = AccountUser.Password;
                AccountUser.Password = se.Encrypt(AccountUser.Password);
                _repository.UpdateObject(AccountUser);
                //AccountUser.Password = realpassword; // set back to unencrypted password to prevent encrypting an already encrypted password on the next update
            }
            return AccountUser;
        }

        public AccountUser SoftDeleteObject(AccountUser AccountUser, int LoggedId)
        {
            return (_validator.ValidDeleteObject(AccountUser, LoggedId) ? _repository.SoftDeleteObject(AccountUser) : AccountUser);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public AccountUser FindOrCreateSysAdmin()
        {
            AccountUser AccountUser = GetObjectByIsAdmin(true);
            if (AccountUser == null)
            {
                AccountUser = CreateObject(Core.Constants.Constant.UserType.Admin, "sysadmin", "Administrator", "Administrator", true,1);
            }
            return AccountUser;
        }

        

        /*public static bool IsAuthenticated()
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

        public static int GetUserTypeId()
        {
            // IF IN MODE TESTING
            if (ConfigurationModels.MODE_TESTING())
                return ConfigurationModels.GET_MODE_TESTING_USERTYPEID();

            int userTypeId = 0;
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
                        {
                            string strUserTypeId = userDetail[1];
                            userTypeId = int.Parse(strUserTypeId);
                        }
                    }
                }
            }
            return userTypeId;
        }//end function GetUserTypeId

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
                        if (!String.IsNullOrEmpty(userDetail[2]))
                            userName = userDetail[2];
                    }
                }
            }
            return userName;
        }//end function GetUserName
        */

    }
}
