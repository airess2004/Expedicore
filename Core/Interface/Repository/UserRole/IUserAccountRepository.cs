using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IAccountUserRepository : IRepository<AccountUser>
    {
        IQueryable<AccountUser> GetQueryable();
        IList<AccountUser> GetAll();
        AccountUser GetObjectById(int Id);
        AccountUser GetObjectByIsAdmin(bool IsAdmin);
        AccountUser GetObjectByUsername(string username);
        AccountUser IsLoginValid(string username, string password);
        AccountUser CreateObject(AccountUser AccountUser);
        AccountUser UpdateObject(AccountUser AccountUser);
        AccountUser SoftDeleteObject(AccountUser AccountUser);
        bool DeleteObject(int Id);
    }
}