using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Interface.Validation;
using Core.DomainModel;
using Core.Interface.Service;
using Core.Constants;

namespace Validation.Validation
{
    public class AccessUserValidator : IAccessUserValidator
    {

        public AccessUser VIsValidAccountUser(AccessUser accessUser, IAccountUserService _AccountUserService)
        {
            AccountUser AccountUser = _AccountUserService.GetObjectById(accessUser.AccountUserId);
            if (AccountUser == null)
            {
                accessUser.Errors.Add("Generic", "AccountUser Tidak valid");
            }
            return accessUser;
        }

        public AccessUser VIsValidMenuUser(AccessUser accessUser, IMenuUserService _menuUserService)
        {
            MenuUser menuUser = _menuUserService.GetObjectById(accessUser.MenuUserId);
            if (menuUser == null)
            {
                accessUser.Errors.Add("Generic", "MenuUser Tidak valid");
            }
            return accessUser;
        }

        public AccessUser VHasUniqueMenuUserAndAccountUserCombination(AccessUser accessUser, IAccessUserService _accessUserService)
        {
            AccessUser accessUser2 = _accessUserService.GetObjectByAccountUserIdAndMenuUserId(accessUser.AccountUserId, accessUser.MenuUserId);
            if (accessUser2 != null)
            {
                accessUser.Errors.Add("Generic", "Kombinasi AccountUser dan MenuUser sudah ada");
            }
            return accessUser;
        }

        public AccessUser VCreateObject(AccessUser accessUser, IAccountUserService _AccountUserService, IMenuUserService _menuUserService, IAccessUserService _accessUserService)
        {
            VIsValidAccountUser(accessUser, _AccountUserService);
            if (!isValid(accessUser)) { return accessUser; }
            VIsValidMenuUser(accessUser, _menuUserService);
            if (!isValid(accessUser)) { return accessUser; }
            VHasUniqueMenuUserAndAccountUserCombination(accessUser, _accessUserService);
            return accessUser;
        }

        public AccessUser VUpdateObject(AccessUser accessUser, IAccountUserService _AccountUserService, IMenuUserService _menuUserService, IAccessUserService _accessUserService)
        {
            VIsValidAccountUser(accessUser, _AccountUserService);
            if (!isValid(accessUser)) { return accessUser; }
            VIsValidMenuUser(accessUser, _menuUserService);
            return accessUser;
        }

        public AccessUser VDeleteObject(AccessUser accessUser)
        {
            return accessUser;
        }

        public bool ValidCreateObject(AccessUser accessUser, IAccountUserService _AccountUserService, IMenuUserService _menuUserService, IAccessUserService _accessUserService)
        {
            VCreateObject(accessUser, _AccountUserService, _menuUserService, _accessUserService);
            return isValid(accessUser);
        }

        public bool ValidUpdateObject(AccessUser accessUser, IAccountUserService _AccountUserService, IMenuUserService _menuUserService, IAccessUserService _accessUserService)
        {
            VUpdateObject(accessUser, _AccountUserService, _menuUserService, _accessUserService);
            return isValid(accessUser);
        }

        public bool ValidDeleteObject(AccessUser accessUser)
        {
            accessUser.Errors.Clear();
            VDeleteObject(accessUser);
            return isValid(accessUser);
        }

        public bool isValid(AccessUser obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(AccessUser obj)
        {
            string erroroutput = "";
            KeyValuePair<string, string> first = obj.Errors.ElementAt(0);
            erroroutput += first.Key + "," + first.Value;
            foreach (KeyValuePair<string, string> pair in obj.Errors.Skip(1))
            {
                erroroutput += Environment.NewLine;
                erroroutput += pair.Key + "," + pair.Value;
            }
            return erroroutput;
        }

    }
}
