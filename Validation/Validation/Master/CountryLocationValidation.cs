using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validation.Validation
{ 
    public class CountryLocationValidation : ICountryLocationValidation
    {  
        public CountryLocation VName(CountryLocation countrylocation, ICountryLocationService _countrylocationService)
        {
            if (String.IsNullOrEmpty(countrylocation.Name) || countrylocation.Name.Trim() == "")
            {
                countrylocation.Errors.Add("Name", "Tidak boleh kosong");
            }
            else if (_countrylocationService.IsNameDuplicated(countrylocation))
            {
                countrylocation.Errors.Add("Name", "Tidak boleh diduplikasi");
            }
            return countrylocation;
        }

        public CountryLocation VContinent(CountryLocation countrylocation, IContinentService _continentService)
        {
            Continent continent = _continentService.GetObjectById(countrylocation.ContinentId);
            if (continent == null)
            {
                countrylocation.Errors.Add("ContinentId", "Tidak boleh kosong");

            }

            return countrylocation;
        }

        public CountryLocation VAbbrevation(CountryLocation countrylocation, ICountryLocationService _countrylocationService)
        {
            if (String.IsNullOrEmpty(countrylocation.Abbrevation) || countrylocation.Abbrevation.Trim() == "")
            {
                countrylocation.Errors.Add("Abbrevation", "Tidak boleh kosong");
            }
            return countrylocation;
        }
        
        public CountryLocation VCreateObject(CountryLocation countrylocation, ICountryLocationService _countrylocationService,IContinentService _continentService)
        {
            VName(countrylocation, _countrylocationService);
            if (!isValid(countrylocation)) { return countrylocation; }
            VAbbrevation(countrylocation, _countrylocationService);
            if (!isValid(countrylocation)) { return countrylocation; }
            VContinent(countrylocation, _continentService);
            if (!isValid(countrylocation)) { return countrylocation; }
            return countrylocation;
        }

        public CountryLocation VUpdateObject(CountryLocation countrylocation, ICountryLocationService _countrylocationService,IContinentService _continentService)
        { 
            VName(countrylocation, _countrylocationService);
            if (!isValid(countrylocation)) { return countrylocation; }
            VAbbrevation(countrylocation, _countrylocationService);
            if (!isValid(countrylocation)) { return countrylocation; }
             VContinent(countrylocation, _continentService);
            if (!isValid(countrylocation)) { return countrylocation; }
            return countrylocation;
        }

        public bool isValid(CountryLocation obj)
        { 
            bool isValid = !obj.Errors.Any();
            return isValid;
        }


    }
}
