using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class GroupEmployeeService : IGroupEmployeeService 
    {  
        private IGroupEmployeeRepository _repository;
        private IGroupEmployeeValidation _validator;

        public GroupEmployeeService(IGroupEmployeeRepository _groupemployeeRepository, IGroupEmployeeValidation _groupemployeeValidation)
        {
            _repository = _groupemployeeRepository;
            _validator = _groupemployeeValidation;
        }

        public IQueryable<GroupEmployee> GetQueryable()
        {  
            return _repository.GetQueryable();
        }

        public GroupEmployee GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }
         
        public GroupEmployee CreateObject(GroupEmployee groupemployee)
        {
            groupemployee.Errors = new Dictionary<String, String>();
            if (isValid(_validator.VCreateObject(groupemployee,this)))
            {
                groupemployee.MasterCode = _repository.GetLastMasterCode(groupemployee.OfficeId) + 1;
                groupemployee = _repository.CreateObject(groupemployee);
            }
            return groupemployee;
        }
         
        public GroupEmployee UpdateObject(GroupEmployee groupemployee)
        {
            if (isValid(_validator.VUpdateObject(groupemployee, this)))
            {
                groupemployee = _repository.UpdateObject(groupemployee);
            }
            return groupemployee;
        }
         
        public GroupEmployee SoftDeleteObject(GroupEmployee groupemployee)
        {
            groupemployee = _repository.SoftDeleteObject(groupemployee);
            return groupemployee;
        }

        public bool IsNameDuplicated(GroupEmployee groupemployee)
        {
            return _repository.IsNameDuplicated(groupemployee);
        }


        public bool isValid(GroupEmployee obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }
    }
}
