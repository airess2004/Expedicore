using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IAccountUserService
    {
        IQueryable<AccountUser> GetQueryable();
        IAccountUserValidator GetValidator();
        IList<AccountUser> GetAll();
        AccountUser GetObjectById(int Id);
        AccountUser GetObjectByIsAdmin(bool IsAdmin);
        AccountUser GetObjectByUsername(string username);
        AccountUser IsLoginValid(string username, string password);
        AccountUser FindOrCreateSysAdmin();
        AccountUser CreateObject(AccountUser AccountUser);
        AccountUser CreateObject(string username, string password, string name, string description, bool IsAdmin,int officeid);
        AccountUser UpdateObject(AccountUser AccountUser);
        AccountUser UpdateObjectPassword(AccountUser AccountUser, string OldPassword, string NewPassword, string ConfirmPassword);
        AccountUser SoftDeleteObject(AccountUser AccountUser, int LoggedId);
        bool DeleteObject(int Id);
        
    }
}