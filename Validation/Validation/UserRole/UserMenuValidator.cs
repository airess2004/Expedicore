using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Interface.Validation;
using Core.DomainModel;
using Core.Interface.Service;
using Core.Constants;

namespace Validation.Validation
{
    public class MenuUserValidator : IMenuUserValidator
    {

        public MenuUser VIsValidName(MenuUser menuUser)
        {
            if (menuUser.Name == null || menuUser.Name.Trim() == "")
            {
                menuUser.Errors.Add("Name", "Tidak boleh kosong");
            }
            return menuUser;
        }

        public MenuUser VIsValidGroupName(MenuUser menuUser)
        {
            if (menuUser.GroupName == null || menuUser.GroupName.Trim() == "")
            {
                menuUser.Errors.Add("GroupName", "Tidak boleh kosong");
            }
            return menuUser;
        }

        public MenuUser VHasUniqueNameAndGroupName(MenuUser menuUser, IMenuUserService _menuUserService)
        {
            MenuUser menuUser2 = _menuUserService.GetObjectByNameAndGroupName(menuUser.Name.Trim(), menuUser.GroupName.Trim());
            if (menuUser2 != null && menuUser.Id != menuUser2.Id)
            {
                menuUser.Errors.Add("Generic", "Kombinasi Name dan GroupName sudah ada");
            }
            return menuUser;
        }

        public MenuUser VCreateObject(MenuUser menuUser, IMenuUserService _menuUserService)
        {
            VIsValidName(menuUser);
            if (!isValid(menuUser)) { return menuUser; }
            VIsValidGroupName(menuUser);
            if (!isValid(menuUser)) { return menuUser; }
            VHasUniqueNameAndGroupName(menuUser, _menuUserService);
            return menuUser;
        }

        public MenuUser VDeleteObject(MenuUser menuUser)
        {
            return menuUser;
        }

        public bool ValidCreateObject(MenuUser menuUser, IMenuUserService _menuUserService)
        {
            VCreateObject(menuUser, _menuUserService);
            return isValid(menuUser);
        }

        public bool ValidDeleteObject(MenuUser menuUser)
        {
            menuUser.Errors.Clear();
            VDeleteObject(menuUser);
            return isValid(menuUser);
        }

        public bool isValid(MenuUser obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(MenuUser obj)
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
