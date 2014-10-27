using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IAccessUserValidator
    {
        AccessUser VIsValidAccountUser(AccessUser accessUser, IAccountUserService _AccountUserService);
        AccessUser VIsValidMenuUser(AccessUser accessUser, IMenuUserService _menuUserService);
        AccessUser VHasUniqueMenuUserAndAccountUserCombination(AccessUser accessUser, IAccessUserService _accessUserService);
        AccessUser VCreateObject(AccessUser accessUser, IAccountUserService _AccountUserService, IMenuUserService _menuUserService, IAccessUserService _accessUserService);
        AccessUser VUpdateObject(AccessUser accessUser, IAccountUserService _AccountUserService, IMenuUserService _menuUserService, IAccessUserService _accessUserService);
        AccessUser VDeleteObject(AccessUser accessUser);
        bool ValidCreateObject(AccessUser accessUser, IAccountUserService _AccountUserService, IMenuUserService _menuUserService, IAccessUserService _accessUserService);
        bool ValidUpdateObject(AccessUser accessUser, IAccountUserService _AccountUserService, IMenuUserService _menuUserService, IAccessUserService _accessUserService);
        bool ValidDeleteObject(AccessUser accessUser);
        bool isValid(AccessUser accessUser);
        string PrintError(AccessUser accessUser);
    }
}
