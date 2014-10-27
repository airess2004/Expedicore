using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface ITruckOrderService
    {
        IQueryable<TruckOrder> GetQueryable();
        TruckOrder GetObjectById(int Id);
        TruckOrder CreateObject(TruckOrder truckorder);
        TruckOrder UpdateObject(TruckOrder truckorder); 
        TruckOrder SoftDeleteObject(TruckOrder truckorder);
    }
}