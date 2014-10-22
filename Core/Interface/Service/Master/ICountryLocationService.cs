using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface ICountryLocationService
    { 
        IQueryable<CountryLocation> GetQueryable();
        CountryLocation GetObjectById(int Id);
        CountryLocation CreateObject(CountryLocation countrylocationn, IContinentService _continentService);
        CountryLocation UpdateObject(CountryLocation countrylocationn, IContinentService _continentService);
        CountryLocation SoftDeleteObject(CountryLocation countrylocation);
        bool IsNameDuplicated(CountryLocation countrylocation);
    }
}