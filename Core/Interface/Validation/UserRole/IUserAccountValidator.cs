using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IAccountUserValidator
    {
        AccountUser VIsValidUsername(AccountUser AccountUser);
        AccountUser VIsValidPassword(AccountUser AccountUser);
        AccountUser VIsCorrectOldPassword(AccountUser AccountUser, string OldPassword, IAccountUserService _AccountUserService);
        AccountUser VIsCorrectNewPassword(AccountUser AccountUser, string NewPassword, string ConfirmPassword);
        AccountUser VIsValidDeletedId(AccountUser AccountUser, int LoggedId);
        AccountUser VIsNonAdmin(AccountUser AccountUser);
        AccountUser VHasUniqueUsername(AccountUser AccountUser, IAccountUserService _AccountUserService);
        AccountUser VCreateObject(AccountUser AccountUser, IAccountUserService _AccountUserService);
        AccountUser VUpdateObject(AccountUser AccountUser, IAccountUserService _AccountUserService);
        AccountUser VUpdateObjectPassword(AccountUser AccountUser, string OldPassword, string NewPassword, string ConfirmPassword, IAccountUserService _AccountUserService);
        AccountUser VDeleteObject(AccountUser AccountUser, int LoggedId);
        bool ValidCreateObject(AccountUser AccountUser, IAccountUserService _AccountUserService);
        bool ValidUpdateObject(AccountUser AccountUser, IAccountUserService _AccountUserService);
        bool ValidUpdateObjectPassword(AccountUser AccountUser, string OldPassword, string NewPassword, string ConfirmPassword, IAccountUserService _AccountUserService);
        bool ValidDeleteObject(AccountUser AccountUser, int LoggedId);
        bool isValid(AccountUser AccountUser);
        string PrintError(AccountUser AccountUser);
    }
}
