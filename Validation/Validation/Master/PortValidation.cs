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
    public class PortValidation : IPortValidation
    {  
        public Port VName(Port port, IPortService _portService)
        {
            if (String.IsNullOrEmpty(port.Name) || port.Name.Trim() == "")
            {
                port.Errors.Add("Name", "Tidak boleh kosong");
            }
            else if (_portService.IsNameDuplicated(port))
            {
                port.Errors.Add("Name", "Tidak boleh diduplikasi");
            }
            return port;
        }

        public Port VAbbrevation(Port port, IPortService _portService)
        {
            if (String.IsNullOrEmpty(port.Abbrevation) || port.Abbrevation.Trim() == "")
            {
                port.Errors.Add("Abbrevation", "Tidak boleh kosong");
            }
            return port;
        }

        public Port VCityLocation(Port port, ICityLocationService _citylocationservice)
        { 
            CityLocation citylocation = _citylocationservice.GetObjectById(port.CityLocationId);
            if (citylocation == null)
            {
                port.Errors.Add("City", "Tidak boleh kosong");
            } 
            else if (!VOffice(port.OfficeId,citylocation.OfficeId))
            {
                port.Errors.Add("City", "Invalid City");
            }

            return port;
        }

        public Port VCreateObject(Port port, IPortService _portService, ICityLocationService _citylocationservice)
        {
            VName(port, _portService);
            if (!isValid(port)) { return port; }
            VAbbrevation(port, _portService);
            if (!isValid(port)) { return port; } 
            VCityLocation(port, _citylocationservice);
            if (!isValid(port)) { return port; }
            return port;
        }

        public Port VUpdateObject(Port port, IPortService _portService, ICityLocationService _citylocationservice)
        {
            VObject(port, _portService);
            if (!isValid(port)) { return port; }
            VName(port, _portService);
            if (!isValid(port)) { return port; }
            VAbbrevation(port, _portService);
            if (!isValid(port)) { return port; }
            VCityLocation(port, _citylocationservice);
            if (!isValid(port)) { return port; }
            return port;
        }

        public bool isValid(Port obj)
        { 
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public Port VObject(Port port, IPortService _portService)
        {
            Port oldport = _portService.GetObjectById(port.Id);
            if (oldport == null)
            {
                port.Errors.Add("Generic", "Invalid Data For Update");
            }
            else if (!VOffice(port.OfficeId, oldport.OfficeId))
            {
                port.Errors.Add("Generic", "Invalid Data For Update");
            }
            return port;
        }

        public bool VOffice(int OfficeId, int CekOfficeId)
        {
            return OfficeId == CekOfficeId;
        }

    }
}
