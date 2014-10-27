using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Validation
{
    public interface ITruckValidation
    {
        Truck VCreateObject(Truck truck, ITruckService _truckService);
        Truck VUpdateObject(Truck truck, ITruckService _truckService);
    }
}
