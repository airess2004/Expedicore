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
    public class VesselValidation : IVesselValidation
    {  
        public Vessel VName(Vessel vessel, IVesselService _vesselService)
        {
            if (String.IsNullOrEmpty(vessel.Name) || vessel.Name.Trim() == "")
            {
                vessel.Errors.Add("Name", "Tidak boleh kosong");
            }
            else if (_vesselService.IsNameDuplicated(vessel))
            {
                vessel.Errors.Add("Name", "Tidak boleh diduplikasi");
            }
            return vessel;
        }

        public Vessel VAbbrevation(Vessel vessel, IVesselService _vesselService)
        {
            if (String.IsNullOrEmpty(vessel.Abbrevation) || vessel.Abbrevation.Trim() == "")
            {
                vessel.Errors.Add("Abbrevation", "Tidak boleh kosong");
            }
            return vessel;
        }
        
        public Vessel VCreateObject(Vessel vessel, IVesselService _vesselService)
        {
            VName(vessel, _vesselService);
            if (!isValid(vessel)) { return vessel; }
            VAbbrevation(vessel, _vesselService);
            if (!isValid(vessel)) { return vessel; }
            return vessel;
        }

        public Vessel VUpdateObject(Vessel vessel, IVesselService _vesselService)
        {
            VObject(vessel, _vesselService);
            if (!isValid(vessel)) { return vessel; }
            VName(vessel, _vesselService);
            if (!isValid(vessel)) { return vessel; }
            VAbbrevation(vessel, _vesselService);
            if (!isValid(vessel)) { return vessel; }
            return vessel;
        }

        public bool isValid(Vessel obj)
        { 
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public Vessel VObject(Vessel vessel, IVesselService _vesselService)
        {
            Vessel oldvessel = _vesselService.GetObjectById(vessel.Id);
            if (oldvessel == null)
            {
                vessel.Errors.Add("Generic", "Invalid Data For Update");
            }
            else if (!VOffice(vessel.OfficeId, oldvessel.OfficeId))
            {
                vessel.Errors.Add("Generic", "Invalid Data For Update");
            }
            return vessel;
        }

        public bool VOffice(int OfficeId, int CekOfficeId)
        {
            return OfficeId == CekOfficeId;
        }

    }
}
