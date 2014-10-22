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
    public class CityLocationValidation : ICityLocationValidation
    {  
        public CityLocation VName(CityLocation citylocation, ICityLocationService _citylocationService)
        {
            if (String.IsNullOrEmpty(citylocation.Name) || citylocation.Name.Trim() == "")
            {
                citylocation.Errors.Add("Name", "Tidak boleh kosong");
            }
            else if (_citylocationService.IsNameDuplicated(citylocation))
            {
                citylocation.Errors.Add("Name", "Tidak boleh diduplikasi");
            }
            return citylocation;
        }

        public CityLocation VAbbrevation(CityLocation citylocation)
        {
            if (String.IsNullOrEmpty(citylocation.Abbrevation) || citylocation.Abbrevation.Trim() == "")
            {
                citylocation.Errors.Add("Abbrevation", "Tidak boleh kosong");
            }
            return citylocation;
        }
         
        public CityLocation VCountryLocation(CityLocation citylocation, ICountryLocationService _countrylocationservice)
        {
            CountryLocation countrylocation = _countrylocationservice.GetObjectById(citylocation.CountryLocationId);
            if (countrylocation == null)
            {
                citylocation.Errors.Add("Country", "Tidak boleh kosong");
            }
            else if (!VOffice(citylocation.OfficeId,countrylocation.OfficeId))
            {
                citylocation.Errors.Add("Country", "Invalid Country");
            }
            return citylocation;
        }

        
        public CityLocation VCreateObject(CityLocation citylocation, ICityLocationService _citylocationService, ICountryLocationService _countrylocationservice)
        {
            VName(citylocation, _citylocationService);
            if (!isValid(citylocation)) { return citylocation; }
            VAbbrevation(citylocation);
            if (!isValid(citylocation)) { return citylocation; }
            VCountryLocation(citylocation, _countrylocationservice);
            if (!isValid(citylocation)) { return citylocation; }
            return citylocation;
        }

        public CityLocation VUpdateObject(CityLocation citylocation, ICityLocationService _citylocationService, ICountryLocationService _countrylocationservice)
        {
            VObject(citylocation, _citylocationService);
            if (!isValid(citylocation)) { return citylocation; }
            VName(citylocation, _citylocationService);
            if (!isValid(citylocation)) { return citylocation; }
            VAbbrevation(citylocation);
            if (!isValid(citylocation)) { return citylocation; }
            VCountryLocation(citylocation, _countrylocationservice);
            if (!isValid(citylocation)) { return citylocation; }
            return citylocation;
        } 

        public bool isValid(CityLocation obj)
        { 
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public CityLocation VObject(CityLocation citylocation, ICityLocationService _citylocationService)
        {
            CityLocation oldcitylocation = _citylocationService.GetObjectById(citylocation.Id);
            if (oldcitylocation == null)
            {
                citylocation.Errors.Add("Generic", "Invalid Data For Update");
            }
            else if (!VOffice(citylocation.OfficeId, oldcitylocation.OfficeId))
            {
                citylocation.Errors.Add("Generic", "Invalid Data For Update");
            }
            return citylocation;
        }

        public bool VOffice(int OfficeId, int CekOfficeId)
        {
            return OfficeId == CekOfficeId;
        }
    }
}
