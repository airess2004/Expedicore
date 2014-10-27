using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validation.Validation
{ 
    public class ContainerYardValidation : IContainerYardValidation
    {  
        public ContainerYard VName(ContainerYard containerYard, IContainerYardService _containerYardService)
        {
            if (String.IsNullOrEmpty(containerYard.Name) || containerYard.Name.Trim() == "")
            {
                containerYard.Errors.Add("Name", "Tidak boleh kosong");
            }
            else if (_containerYardService.IsNameDuplicated(containerYard))
            {
                containerYard.Errors.Add("Name", "Tidak boleh diduplikasi");
            }
            return containerYard;
        }

        
        public ContainerYard VCreateObject(ContainerYard containerYard, IContainerYardService _containerYardService)
        {
            VName(containerYard, _containerYardService);
            if (!isValid(containerYard)) { return containerYard; }
            return containerYard;
        }

        public ContainerYard VUpdateObject(ContainerYard containerYard, IContainerYardService _containerYardService)
        {
            VObject(containerYard, _containerYardService);
            if (!isValid(containerYard)) { return containerYard; }
            VName(containerYard, _containerYardService);
            if (!isValid(containerYard)) { return containerYard; }
            return containerYard;
        }

        public bool isValid(ContainerYard obj)
        { 
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public ContainerYard VObject(ContainerYard containerYard, IContainerYardService _containerYardService)
        {
            ContainerYard oldcontainerYard = _containerYardService.GetObjectById(containerYard.Id);
            if (oldcontainerYard == null)
            {
                containerYard.Errors.Add("Generic", "Invalid Data For Update");
            }
            else if (!VOffice(containerYard.OfficeId, oldcontainerYard.OfficeId))
            {
                containerYard.Errors.Add("Generic", "Invalid Data For Update");
            }
            return containerYard;
        }

        public bool VOffice(int OfficeId, int CekOfficeId)
        {
            return OfficeId == CekOfficeId;
        }

    }
}
