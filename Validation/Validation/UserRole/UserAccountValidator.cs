using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Interface.Validation;
using Core.DomainModel;
using Core.Interface.Service;
using Core.Constants;
using Service;

namespace Validation.Validation
{
    public class AccountUserValidator : IAccountUserValidator
    {

        public AccountUser VIsValidUsername(AccountUser AccountUser)
        {
            if (AccountUser.Username == null || AccountUser.Username.Trim() == "")
            {
                AccountUser.Errors.Add("Username", "Tidak boleh kosong");
            }
            return AccountUser;
        }

        public AccountUser VIsValidPassword(AccountUser AccountUser)
        {
            if (AccountUser.Password == null)
            {
                AccountUser.Errors.Add("Password", "Tidak boleh kosong");
            }
            return AccountUser;
        }

        public AccountUser VIsCorrectOldPassword(AccountUser AccountUser, string OldPassword, IAccountUserService _AccountUserService)
        {
            if (OldPassword == null || OldPassword.Trim() == "")
            {
                AccountUser.Errors.Add("Generic", "OldPassword Tidak boleh kosong");
            }
            else
            {
                StringEncryption se = new StringEncryption();
                string encOldPassword = se.Encrypt(OldPassword);
                AccountUser AccountUser2 = _AccountUserService.GetObjectById(AccountUser.Id);
                if (encOldPassword != AccountUser2.Password)
                {
                    AccountUser.Errors.Add("Generic", "Old Password Salah");
                }
            }
            return AccountUser;
        }

        public AccountUser VIsCorrectNewPassword(AccountUser AccountUser, string NewPassword, string ConfirmPassword)
        {
            if (NewPassword == null || NewPassword.Trim() == "")
            {
                AccountUser.Errors.Add("Generic", "New Password Tidak boleh kosong");
            }
            else
            {
                if (NewPassword.Trim() != ConfirmPassword.Trim())
                {
                    AccountUser.Errors.Add("Generic", "Confirm Password tidak sama dengan New Password");
                }
            }
            return AccountUser;
        }

        public AccountUser VHasUniqueUsername(AccountUser AccountUser, IAccountUserService _AccountUserService)
        {
            AccountUser AccountUser2 = _AccountUserService.GetObjectByUsername(AccountUser.Username.Trim());
            if (AccountUser2 != null && AccountUser.Id != AccountUser2.Id)
            {
                AccountUser.Errors.Add("Username", "Sudah ada");
            }
            return AccountUser;
        }

        public AccountUser VIsValidDeletedId(AccountUser AccountUser, int LoggedId)
        {
            if (AccountUser.Id == LoggedId)
            {
                AccountUser.Errors.Add("Generic", "Tidak boleh menghapus account anda sendiri");
            }
            return AccountUser;
        }

        public AccountUser VIsNonAdmin(AccountUser AccountUser)
        {
            if (AccountUser.IsAdmin)
            {
                AccountUser.Errors.Add("Generic", "Tidak boleh Admin");
            }
            return AccountUser;
        }

        public AccountUser VCreateObject(AccountUser AccountUser, IAccountUserService _AccountUserService)
        {
            VIsValidUsername(AccountUser);
            if (!isValid(AccountUser)) { return AccountUser; }
            VIsValidPassword(AccountUser);
            if (!isValid(AccountUser)) { return AccountUser; }
            VHasUniqueUsername(AccountUser, _AccountUserService);
            return AccountUser;
        }

        public AccountUser VUpdateObject(AccountUser AccountUser, IAccountUserService _AccountUserService)
        {
            VIsNonAdmin(AccountUser);
            if (!isValid(AccountUser)) { return AccountUser; }
            VCreateObject(AccountUser, _AccountUserService);
            return AccountUser;
        }

        public AccountUser VUpdateObjectPassword(AccountUser AccountUser, string OldPassword, string NewPassword, string ConfirmPassword, IAccountUserService _AccountUserService)
        {
            VCreateObject(AccountUser, _AccountUserService);
            if (!isValid(AccountUser)) { return AccountUser; }
            VIsCorrectNewPassword(AccountUser, NewPassword, ConfirmPassword);
            if (!isValid(AccountUser)) { return AccountUser; }
            VIsCorrectOldPassword(AccountUser, OldPassword, _AccountUserService);
            return AccountUser;
        }

        public AccountUser VDeleteObject(AccountUser AccountUser, int LoggedId)
        {
            VIsValidDeletedId(AccountUser, LoggedId);
            return AccountUser;
        }

        public bool ValidCreateObject(AccountUser AccountUser, IAccountUserService _AccountUserService)
        {
            VCreateObject(AccountUser, _AccountUserService);
            return isValid(AccountUser);
        }

        public bool ValidUpdateObjectPassword(AccountUser AccountUser, string OldPassword, string NewPassword, string ConfirmPassword, IAccountUserService _AccountUserService)
        {
            VUpdateObjectPassword(AccountUser, OldPassword, NewPassword, ConfirmPassword, _AccountUserService);
            return isValid(AccountUser);
        }

        public bool ValidUpdateObject(AccountUser AccountUser, IAccountUserService _AccountUserService)
        {
            VUpdateObject(AccountUser, _AccountUserService);
            return isValid(AccountUser);
        }

        public bool ValidDeleteObject(AccountUser AccountUser, int LoggedId)
        {
            AccountUser.Errors.Clear();
            VDeleteObject(AccountUser, LoggedId);
            return isValid(AccountUser);
        }

        public bool isValid(AccountUser obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(AccountUser obj)
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
