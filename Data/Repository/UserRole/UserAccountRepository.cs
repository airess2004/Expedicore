using Core.DomainModel;
using Core.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data.Context;
using System.Data;

namespace Data.Repository
{
    public class AccountUserRepository : EfRepository<AccountUser>, IAccountUserRepository
    {
        private ExpedicoEntities entities;
        public AccountUserRepository()
        {
            entities = new ExpedicoEntities();
        }

        public IQueryable<AccountUser> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<AccountUser> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public AccountUser GetObjectById(int Id)
        {
            AccountUser userAccount = Find(x => x.Id == Id && !x.IsDeleted);
            if (userAccount != null) { userAccount.Errors = new Dictionary<string, string>(); }
            return userAccount;
        }

        public AccountUser GetObjectByIsAdmin(bool IsAdmin)
        {
            AccountUser userAccount = FindAll(x => x.IsAdmin == IsAdmin && !x.IsDeleted).FirstOrDefault();
            if (userAccount != null) { userAccount.Errors = new Dictionary<string, string>(); }
            return userAccount;
        }

        public AccountUser GetObjectByUsername(string username)
        {
            string lowerusername = username.ToLower();
            AccountUser userAccount = Find(x => x.Username.ToLower() == lowerusername && !x.IsDeleted);
            if (userAccount != null) { userAccount.Errors = new Dictionary<string, string>(); }
            return userAccount;
        }

        public AccountUser IsLoginValid(string username, string password)
        {
            string lowerusername = username.ToLower();
            AccountUser userAccount = Find(x => x.Username.ToLower() == lowerusername && x.Password == password && !x.IsDeleted);
            if (userAccount != null) { userAccount.Errors = new Dictionary<string, string>(); }
            return userAccount;
        }

        public AccountUser CreateObject(AccountUser userAccount)
        {
            userAccount.IsDeleted = false;
            userAccount.CreatedAt = DateTime.Now;
            return Create(userAccount);
        }

        public AccountUser UpdateObject(AccountUser userAccount)
        {
            userAccount.UpdatedAt = DateTime.Now;
            Update(userAccount);
            return userAccount;
        }

        public AccountUser SoftDeleteObject(AccountUser userAccount)
        {
            userAccount.IsDeleted = true;
            userAccount.DeletedAt = DateTime.Now;
            Update(userAccount);
            return userAccount;
        }

        public bool DeleteObject(int Id)
        {
            AccountUser userAccount = Find(x => x.Id == Id);
            return (Delete(userAccount) == 1) ? true : false;
        }

    }
}