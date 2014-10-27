using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IMenuUserService
    {
        IQueryable<MenuUser> GetQueryable();
        IMenuUserValidator GetValidator();
        IList<MenuUser> GetAll();
        MenuUser GetObjectById(int Id);
        MenuUser GetObjectByNameAndGroupName(string Name, string GroupName);
        MenuUser CreateObject(MenuUser menuUser);
        MenuUser CreateObject(string Name, string GroupName);
        MenuUser SoftDeleteObject(MenuUser menuUser);
        bool DeleteObject(int Id);
    }
}