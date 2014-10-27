using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IAccessUserService
    {
        IAccessUserValidator GetValidator();
        IQueryable<AccessUser> GetQueryable();
        IQueryable<AccessUser> GetQueryableObjectsByAccountUserId(int AccountUserId);
        IList<AccessUser> GetAll();
        IList<AccessUser> GetObjectsByAccountUserId(int AccountUserId);
        AccessUser GetObjectByAccountUserIdAndMenuUserId(int AccountUserId, int MenuUserId);
        AccessUser GetObjectById(int Id);
        AccessUser CreateObject(AccessUser accessUser, IAccountUserService _AccountUserService, IMenuUserService _menuUserService);
        AccessUser UpdateObject(AccessUser accessUser, IAccountUserService _AccountUserService, IMenuUserService _menuUserService);
        AccessUser SoftDeleteObject(AccessUser accessUser);
        bool DeleteObject(int Id);
        int CreateDefaultAccess(int AccountUserId, IMenuUserService _menuUserService, IAccountUserService _AccountUserService);
    }
}