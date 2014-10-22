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
    public class AirlineValidation : IAirlineValidation
    {  
        public Airline VName(Airline airline, IAirlineService _airlineService)
        {
            if (String.IsNullOrEmpty(airline.Name) || airline.Name.Trim() == "")
            {
                airline.Errors.Add("Name", "Tidak boleh kosong");
            }
            else if (_airlineService.IsNameDuplicated(airline))
            {
                airline.Errors.Add("Name", "Tidak boleh diduplikasi");
            }
            return airline;
        }

        public Airline VAbbrevation(Airline airline)
        {
            if (String.IsNullOrEmpty(airline.Abbrevation) || airline.Abbrevation.Trim() == "")
            {
                airline.Errors.Add("Abbrevation", "Tidak boleh kosong");
            }
            return airline;
        }
        
        public Airline VCreateObject(Airline airline, IAirlineService _airlineService)
        {
            VName(airline, _airlineService);
            if (!isValid(airline)) { return airline; }
            VAbbrevation(airline);
            if (!isValid(airline)) { return airline; }
            return airline;
        }

        public Airline VUpdateObject(Airline airline, IAirlineService _airlineService)
        {
            VObject(airline, _airlineService);
            if (!isValid(airline)) { return airline; }
            VName(airline, _airlineService);
            if (!isValid(airline)) { return airline; }
            VAbbrevation(airline);
            if (!isValid(airline)) { return airline; }
            return airline;
        }
       
        public Airline VObject(Airline airline, IAirlineService _airlineService)
        {
            Airline oldairline = _airlineService.GetObjectById(airline.Id);
            if (oldairline == null)
            {
                airline.Errors.Add("Generic", "Invalid Data For Update");
            }
            else if (!VOffice(airline.OfficeId,oldairline.OfficeId))
            {
                airline.Errors.Add("Generic", "Invalid Data For Update");
            }
            return airline;
        }


        public bool VOffice(int OfficeId, int CekOfficeId)
        {
            return OfficeId == CekOfficeId;
        }

        public bool isValid(Airline obj)
        { 
            bool isValid = !obj.Errors.Any();
            return isValid;
        }
    }
}
