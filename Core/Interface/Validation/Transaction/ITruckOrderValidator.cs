using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Validation
{
    public interface ITruckOrderValidation
    { 
        TruckOrder VCreateObject(TruckOrder truckorder, ITruckOrderService _truckorderService);
        TruckOrder VUpdateObject(TruckOrder truckorder, ITruckOrderService _truckorderService);
    }
}
