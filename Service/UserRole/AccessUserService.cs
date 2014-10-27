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
    public class AccessUserService : IAccessUserService
    {
        private IAccessUserRepository _repository;
        private IAccessUserValidator _validator;

        public AccessUserService(IAccessUserRepository _accessUserRepository, IAccessUserValidator _accessUserValidator)
        {
            _repository = _accessUserRepository;
            _validator = _accessUserValidator;
        }

        public IAccessUserValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<AccessUser> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IQueryable<AccessUser> GetQueryableObjectsByAccountUserId(int AccountUserId)
        {
            return _repository.GetQueryableObjectsByAccountUserId(AccountUserId);
        }

        public IList<AccessUser> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<AccessUser> GetObjectsByAccountUserId(int AccountUserId)
        {
            return _repository.GetObjectsByAccountUserId(AccountUserId);
        }

        public AccessUser GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public AccessUser GetObjectByAccountUserIdAndMenuUserId(int AccountUserId, int MenuUserId)
        {
            return _repository.GetObjectByAccountUserIdAndMenuUserId(AccountUserId, MenuUserId);
        }

        public AccessUser CreateObject(AccessUser accessUser, IAccountUserService _AccountUserService, IMenuUserService _menuUserService)
        {
            accessUser.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(accessUser, _AccountUserService, _menuUserService, this) ? _repository.CreateObject(accessUser) : accessUser);
        }

        public AccessUser UpdateObject(AccessUser accessUser, IAccountUserService _AccountUserService, IMenuUserService _menuUserService)
        {
            return (_validator.ValidUpdateObject(accessUser, _AccountUserService, _menuUserService, this) ? _repository.UpdateObject(accessUser) : accessUser);
        }

        public AccessUser SoftDeleteObject(AccessUser accessUser)
        {
            return (_validator.ValidDeleteObject(accessUser) ? _repository.SoftDeleteObject(accessUser) : accessUser);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public int CreateDefaultAccess(int AccountUserId, IMenuUserService _menuUserService, IAccountUserService _AccountUserService)
        {
            var menuUsers = _menuUserService.GetAll();
            int count = 0;
            foreach (var menuUser in menuUsers)
            {
                AccessUser accessUser = new AccessUser()
                {
                    AccountUserId = AccountUserId,
                    MenuUserId = menuUser.Id,
                };
                AccountUser AccountUser = _AccountUserService.GetObjectById(AccountUserId);
                if (AccountUser.IsAdmin)
                {
                    accessUser.AllowConfirm = true;
                    accessUser.AllowCreate = true;
                    accessUser.AllowDelete = true;
                    accessUser.AllowEdit = true;
                    accessUser.AllowPaid = true;
                    accessUser.AllowPrint = true;
                    accessUser.AllowReconcile = true;
                    accessUser.AllowUnconfirm = true;
                    accessUser.AllowUndelete = true;
                    accessUser.AllowUnpaid = true;
                    accessUser.AllowUnreconcile = true;
                    accessUser.AllowView = true;
                }
                CreateObject(accessUser, _AccountUserService, _menuUserService);
                if (!accessUser.Errors.Any()) count++;
            }
            return count;
        }

    }
}
