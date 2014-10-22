using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Validation
{
    public interface ICityLocationValidation
    {
        CityLocation VCreateObject(CityLocation citylocation, ICityLocationService _citylocationService, ICountryLocationService _countrylocationservice);
        CityLocation VUpdateObject(CityLocation citylocation, ICityLocationService _citylocationService, ICountryLocationService _countrylocationservice);
    }
}
