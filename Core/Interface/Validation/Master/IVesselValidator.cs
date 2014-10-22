using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Validation
{
    public interface IVesselValidation
    {
        Vessel VCreateObject(Vessel vessel, IVesselService _vesselService);
        Vessel VUpdateObject(Vessel vessel, IVesselService _vesselService);
    }
}
