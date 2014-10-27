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
    public class MenuUserRepository : EfRepository<MenuUser>, IMenuUserRepository
    {
        private ExpedicoEntities entities;
        public MenuUserRepository()
        {
            entities = new ExpedicoEntities();
        }

        public IQueryable<MenuUser> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<MenuUser> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public MenuUser GetObjectById(int Id)
        {
            MenuUser userMenu = Find(x => x.Id == Id && !x.IsDeleted);
            if (userMenu != null) { userMenu.Errors = new Dictionary<string, string>(); }
            return userMenu;
        }

        public MenuUser GetObjectByNameAndGroupName(string Name, string GroupName)
        {
            MenuUser userMenu = Find(x => x.Name == Name && x.GroupName == GroupName && !x.IsDeleted);
            if (userMenu != null) { userMenu.Errors = new Dictionary<string, string>(); }
            return userMenu;
        }

        public MenuUser CreateObject(MenuUser userMenu)
        {
            userMenu.IsDeleted = false;
            userMenu.CreatedAt = DateTime.Now;
            return Create(userMenu);
        }

        public MenuUser SoftDeleteObject(MenuUser userMenu)
        {
            userMenu.IsDeleted = true;
            userMenu.DeletedAt = DateTime.Now;
            Update(userMenu);
            return userMenu;
        }

        public bool DeleteObject(int Id)
        {
            MenuUser userMenu = Find(x => x.Id == Id);
            return (Delete(userMenu) == 1) ? true : false;
        }

    }
}