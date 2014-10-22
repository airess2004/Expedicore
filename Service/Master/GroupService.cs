using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Master
{
    public class GroupService : IGroupService 
    {  
        private IGroupRepository _repository;
        private IGroupValidation _validator;

        public GroupService(IGroupRepository _groupRepository, IGroupValidation _groupValidation)
        {
            _repository = _groupRepository;
            _validator = _groupValidation;
        }

        public IQueryable<Group> GetQueryable()
        {  
            return _repository.GetQueryable();
        }

        public Group GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }
         
        public Group CreateObject(Group group)
        {
            group.Errors = new Dictionary<String, String>();
            if (!isValid(_validator.VCreateObject(group,this)))
            {
                group.MasterCode = _repository.GetLastMasterCode(group.OfficeId) + 1;
                group = _repository.CreateObject(group);
            }
            return group;
        }
         
        public Group UpdateObject(Group group)
        {
            if (!isValid(_validator.VUpdateObject(group, this)))
            {
                group = _repository.UpdateObject(group);
            }
            return group;
        }
         
        public Group SoftDeleteObject(Group group)
        {
            group = _repository.SoftDeleteObject(group);
            return group;
        }

        public bool IsNameDuplicated(Group group)
        {
            return _repository.IsNameDuplicated(group);
        }


        public bool isValid(Group obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }
    }
}
