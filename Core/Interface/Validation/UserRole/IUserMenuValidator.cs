using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IMenuUserValidator
    {
        MenuUser VIsValidName(MenuUser menuUser);
        MenuUser VIsValidGroupName(MenuUser menuUser);
        MenuUser VHasUniqueNameAndGroupName(MenuUser menuUser, IMenuUserService _menuUserService);
        MenuUser VCreateObject(MenuUser menuUser, IMenuUserService _menuUserService);
        MenuUser VDeleteObject(MenuUser menuUser);
        bool ValidCreateObject(MenuUser menuUser, IMenuUserService _menuUserService);
        bool ValidDeleteObject(MenuUser menuUser);
        bool isValid(MenuUser menuUser);
        string PrintError(MenuUser menuUser);
    }
}
