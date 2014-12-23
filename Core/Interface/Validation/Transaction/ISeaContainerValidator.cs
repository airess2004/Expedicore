using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Validation
{
    public interface ISeaContainerValidation
    {
        SeaContainer VCreateObject(SeaContainer seacontainer, ISeaContainerService _seacontainerService);
        SeaContainer VUpdateObject(SeaContainer seacontainer, ISeaContainerService _seacontainerService);
    }
}
