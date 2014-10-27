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
    public class ContainerYardService : IContainerYardService 
    {  
        private IContainerYardRepository _repository;
        private IContainerYardValidation _validator;

        public ContainerYardService(IContainerYardRepository _containerYardRepository, IContainerYardValidation _containerYardValidation)
        {
            _repository = _containerYardRepository;
            _validator = _containerYardValidation;
        }

        public IQueryable<ContainerYard> GetQueryable()
        {  
            return _repository.GetQueryable();
        }

        public ContainerYard GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }
         
        public ContainerYard CreateObject(ContainerYard containerYard)
        {
            containerYard.Errors = new Dictionary<String, String>();
            if (isValid(_validator.VCreateObject(containerYard,this)))
            {
                containerYard.MasterCode = _repository.GetLastMasterCode(containerYard.OfficeId) + 1;
                containerYard = _repository.CreateObject(containerYard);
            }
            return containerYard;
        }
         
        public ContainerYard UpdateObject(ContainerYard containerYard)
        {
            if (isValid(_validator.VUpdateObject(containerYard, this)))
            {
                containerYard = _repository.UpdateObject(containerYard);
            }
            return containerYard;
        }
         
        public ContainerYard SoftDeleteObject(ContainerYard containerYard)
        {
            containerYard = _repository.SoftDeleteObject(containerYard);
            return containerYard;
        }

        public bool IsNameDuplicated(ContainerYard containerYard)
        {
            return _repository.IsNameDuplicated(containerYard);
        }


        public bool isValid(ContainerYard obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }
    }
}
