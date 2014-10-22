using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Validation
{
    public interface IPortValidation
    {
        Port VCreateObject(Port port, IPortService _portService, ICityLocationService _citylocationservice);
        Port VUpdateObject(Port port, IPortService _portService, ICityLocationService _citylocationservice);
    }
}
