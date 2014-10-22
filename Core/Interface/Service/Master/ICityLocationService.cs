using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface ICityLocationService
    {
        IQueryable<CityLocation> GetQueryable();
        CityLocation GetObjectById(int Id);
        CityLocation CreateObject(CityLocation citylocation, ICountryLocationService _countrylocationService);
        CityLocation UpdateObject(CityLocation citylocation, ICountryLocationService _countrylocationService);
        CityLocation SoftDeleteObject(CityLocation citylocation);
        bool IsNameDuplicated(CityLocation citylocation);
    }
}