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
    public class AirportValidation : IAirportValidation
    {  
        public Airport VName(Airport airport, IAirportService _airportService)
        {
            if (String.IsNullOrEmpty(airport.Name) || airport.Name.Trim() == "")
            {
                airport.Errors.Add("Name", "Tidak boleh kosong");
            }
            else if (_airportService.IsNameDuplicated(airport))
            {
                airport.Errors.Add("Name", "Tidak boleh diduplikasi");
            }
            return airport;
        }

        public Airport VAbbrevation(Airport airport)
        {
            if (String.IsNullOrEmpty(airport.Abbrevation) || airport.Abbrevation.Trim() == "")
            {
                airport.Errors.Add("Abbrevation", "Tidak boleh kosong");
            }
            return airport;
        }  

        public Airport VCityLocation(Airport airport, ICityLocationService _citylocationservice)
        {
            CityLocation citylocation = _citylocationservice.GetObjectById(airport.CityLocationId);
            if (citylocation == null)
            {
                airport.Errors.Add("City", "Tidak boleh kosong");
            }
            else if(!VOffice(airport.OfficeId,citylocation.OfficeId))
            {
                airport.Errors.Add("City", "Invalid City");
            }
            return airport;
        }

        public Airport VCreateObject(Airport airport, IAirportService _airportService, ICityLocationService _citylocationservice)
        {
            VName(airport, _airportService);
            if (!isValid(airport)) { return airport; }
            VAbbrevation(airport);
            if (!isValid(airport)) { return airport; }
            VCityLocation(airport, _citylocationservice);
            if (!isValid(airport)) { return airport; }
            return airport;
        }
         
        public Airport VUpdateObject(Airport airport, IAirportService _airportService, ICityLocationService _citylocationservice)
        {
            VObject(airport, _airportService);
            if (!isValid(airport)) { return airport; }
            VName(airport, _airportService);
            if (!isValid(airport)) { return airport; }
            VAbbrevation(airport);
            if (!isValid(airport)) { return airport; }
            VCityLocation(airport, _citylocationservice);
            if (!isValid(airport)) { return airport; }
            return airport;
        }

        public Airport VObject(Airport airport, IAirportService _airportService)
        {
            Airport oldairport = _airportService.GetObjectById(airport.Id);
            if (oldairport == null)
            {
                airport.Errors.Add("Generic", "Invalid Data For Update");
            }
            else if (!VOffice(airport.OfficeId, oldairport.OfficeId))
            {
                airport.Errors.Add("Generic", "Invalid Data For Update");
            }
            return airport;
        }


        public bool VOffice(int OfficeId, int CekOfficeId)
        {
            return OfficeId == CekOfficeId;
        }

        public bool isValid(Airport obj)
        { 
            bool isValid = !obj.Errors.Any();
            return isValid;
        }
    }
}
