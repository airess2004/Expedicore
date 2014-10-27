using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IMenuUserRepository : IRepository<MenuUser>
    {
        IQueryable<MenuUser> GetQueryable();
        IList<MenuUser> GetAll();
        MenuUser GetObjectById(int Id);
        MenuUser GetObjectByNameAndGroupName(string Name, string GroupName);
        MenuUser CreateObject(MenuUser menuUser);
        MenuUser SoftDeleteObject(MenuUser menuUser);
        bool DeleteObject(int Id);
    }
}