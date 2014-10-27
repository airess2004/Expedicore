using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface ITruckService
    {
        IQueryable<Truck> GetQueryable();
        Truck GetObjectById(int Id);
        Truck CreateObject(Truck truck);
        Truck UpdateObject(Truck truck);
        Truck SoftDeleteObject(Truck truck);
        bool IsNameDuplicated(Truck truck);
    }
}