using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IAirlineService
    {
        IQueryable<Airline> GetQueryable();
        Airline GetObjectById(int Id);
        Airline CreateObject(Airline airline);
        Airline UpdateObject(Airline airline);
        Airline SoftDeleteObject(Airline airline);
        bool IsNameDuplicate(Airline airline);

    }
}