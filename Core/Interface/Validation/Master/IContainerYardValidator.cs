using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Validation
{
    public interface IContainerYardValidation
    {
        ContainerYard VCreateObject(ContainerYard containeryard, IContainerYardService _containeryardService);
        ContainerYard VUpdateObject(ContainerYard containeryard, IContainerYardService _containeryardService);
    }
}
