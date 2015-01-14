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
    public class AccessUserRepository : EfRepository<AccessUser>, IAccessUserRepository
    {
        private ExpedicoEntities entities;
        public AccessUserRepository()
        {
            entities = new ExpedicoEntities();
        }

        public IQueryable<AccessUser> GetQueryable()
        {
            return FindAll();
        }

        public IQueryable<AccessUser> GetQueryableObjectsByAccountUserId(int AccountUserId)
        {
            return FindAll(x => !x.IsDeleted && x.AccountUserId == AccountUserId);
        }

        public IList<AccessUser> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public IList<AccessUser> GetObjectsByAccountUserId(int AccountUserId)
        {
            return FindAll(x => !x.IsDeleted && x.AccountUserId == AccountUserId).ToList();
        }

        public AccessUser GetObjectById(int Id)
        {
            AccessUser userAccess = Find(x => x.Id == Id && !x.IsDeleted);
            if (userAccess != null) { userAccess.Errors = new Dictionary<string, string>(); }
            return userAccess;
        }

        public AccessUser GetObjectByAccountUserIdAndMenuUserId(int AccountUserId, int MenuUserId)
        {
            AccessUser userAccess = Find(x => x.AccountUserId == AccountUserId && x.MenuUserId == MenuUserId && !x.IsDeleted);
            if (userAccess != null) { userAccess.Errors = new Dictionary<string, string>(); }
            return userAccess;
        }

        public AccessUser CreateObject(AccessUser userAccess)
        {
            userAccess.IsDeleted = false;
            userAccess.CreatedAt = DateTime.Now;
            return Create(userAccess);
        }

        public AccessUser UpdateObject(AccessUser userAccess)
        {
            userAccess.UpdatedAt = DateTime.Now;
            Update(userAccess);
            return userAccess;
        }

        public AccessUser SoftDeleteObject(AccessUser userAccess)
        {
            userAccess.IsDeleted = true;
            userAccess.DeletedAt = DateTime.Now;
            Update(userAccess);
            return userAccess;
        }

        public bool DeleteObject(int Id)
        {
            AccessUser userAccess = Find(x => x.Id == Id);
            return (Delete(userAccess) == 1) ? true : false;
        }

    }
}