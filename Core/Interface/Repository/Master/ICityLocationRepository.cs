using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface ICityLocationRepository : IRepository<CityLocation>
    {
       IQueryable<CityLocation> GetQueryable();
       CityLocation GetObjectById(int Id);
       CityLocation CreateObject(CityLocation model);
       CityLocation UpdateObject(CityLocation model);
       CityLocation SoftDeleteObject(CityLocation model);
       bool DeleteObject(int Id);  
    }
}