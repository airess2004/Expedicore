using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IAccessUserRepository : IRepository<AccessUser>
    {
        IQueryable<AccessUser> GetQueryable();
        IQueryable<AccessUser> GetQueryableObjectsByAccountUserId(int AccountUserId);
        IList<AccessUser> GetAll();
        IList<AccessUser> GetObjectsByAccountUserId(int AccountUserId);
        AccessUser GetObjectByAccountUserIdAndMenuUserId(int AccountUserId, int MenuUserId);
        AccessUser GetObjectById(int Id);
        AccessUser CreateObject(AccessUser accessUser);
        AccessUser UpdateObject(AccessUser accessUser);
        AccessUser SoftDeleteObject(AccessUser accessUser);
        bool DeleteObject(int Id);
    }
}