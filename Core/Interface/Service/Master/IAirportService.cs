using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IAirportService
    {
        IQueryable<Airport> GetQueryable();
        Airport GetObjectById(int Id);
        Airport CreateObject(Airport airport, ICityLocationService _citylocationservice);
        Airport UpdateObject(Airport airport, ICityLocationService _citylocationservice);
        Airport SoftDeleteObject(Airport airport);
        bool IsNameDuplicated(Airport airport);
    }
}