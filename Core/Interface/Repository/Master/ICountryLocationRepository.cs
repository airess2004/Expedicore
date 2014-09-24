using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface ICountryLocationRepository : IRepository<CountryLocation>
    {
       IQueryable<CountryLocation> GetQueryable();
       CountryLocation GetObjectById(int Id);
       CountryLocation CreateObject(CountryLocation model);
       CountryLocation UpdateObject(CountryLocation model);
       CountryLocation SoftDeleteObject(CountryLocation model);
       bool DeleteObject(int Id);  
    }
}