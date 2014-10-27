using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Interface.Validation;

namespace Service
{
    public class MenuUserService : IMenuUserService
    {
        private IMenuUserRepository _repository;
        private IMenuUserValidator _validator;

        public MenuUserService(IMenuUserRepository _menuUserRepository, IMenuUserValidator _menuUserValidator)
        {
            _repository = _menuUserRepository;
            _validator = _menuUserValidator;
        }

        public IMenuUserValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<MenuUser> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<MenuUser> GetAll()
        {
            return _repository.GetAll();
        }

        public MenuUser GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public MenuUser GetObjectByNameAndGroupName(string Name, string GroupName)
        {
            return _repository.GetObjectByNameAndGroupName(Name, GroupName);
        }

        public MenuUser CreateObject(MenuUser menuUser)
        {
            menuUser.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(menuUser, this) ? _repository.CreateObject(menuUser) : menuUser);
        }

        public MenuUser CreateObject(string Name, string GroupName)
        {
            MenuUser menuUser = new MenuUser()
            {
                Name = Name,
                GroupName = GroupName
            };
            return CreateObject(menuUser);
        }

        public MenuUser SoftDeleteObject(MenuUser menuUser)
        {
            return (_validator.ValidDeleteObject(menuUser) ? _repository.SoftDeleteObject(menuUser) : menuUser);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }
    }
}
