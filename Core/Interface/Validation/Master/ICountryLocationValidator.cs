using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Validation
{
    public interface ICountryLocationValidation
    {
        CountryLocation VCreateObject(CountryLocation countrylocation, ICountryLocationService _countrylocationService,IContinentService _continentService);
        CountryLocation VUpdateObject(CountryLocation countrylocation, ICountryLocationService _countrylocationService, IContinentService _continentService);
    }
}
