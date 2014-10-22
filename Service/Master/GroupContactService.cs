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
    public class GroupContactService : IGroupContactService 
    {  
        private IGroupContactRepository _repository;
        private IGroupContactValidation _validator;

        public GroupContactService(IGroupContactRepository _groupcontactRepository, IGroupContactValidation _groupcontactValidation)
        {
            _repository = _groupcontactRepository;
            _validator = _groupcontactValidation;
        }

        public IQueryable<GroupContact> GetQueryable()
        {  
            return _repository.GetQueryable();
        }

        public GroupContact GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }
         
        public GroupContact CreateObject(GroupContact groupcontact)
        {
            groupcontact.Errors = new Dictionary<String, String>();
            if (!isValid(_validator.VCreateObject(groupcontact,this)))
            {
                groupcontact.MasterCode = _repository.GetLastMasterCode(groupcontact.OfficeId) + 1;
                groupcontact = _repository.CreateObject(groupcontact);
            }
            return groupcontact;
        }
         
        public GroupContact UpdateObject(GroupContact groupcontact)
        {
            if (!isValid(_validator.VUpdateObject(groupcontact, this)))
            {
                groupcontact = _repository.UpdateObject(groupcontact);
            }
            return groupcontact;
        }
         
        public GroupContact SoftDeleteObject(GroupContact groupcontact)
        {
            groupcontact = _repository.SoftDeleteObject(groupcontact);
            return groupcontact;
        }

        public bool IsNameDuplicated(GroupContact groupcontact)
        {
            return _repository.IsNameDuplicated(groupcontact);
        }


        public bool isValid(GroupContact obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }
    }
}
