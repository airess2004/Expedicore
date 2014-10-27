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
    public class TruckValidation : ITruckValidation
    {  
        public Truck VName(Truck truck, ITruckService _truckService)
        {
            if (String.IsNullOrEmpty(truck.NoPlat) || truck.NoPlat.Trim() == "")
            {
                truck.Errors.Add("NoPlat", "Tidak boleh kosong");
            }
            else if (_truckService.IsNameDuplicated(truck))
            {
                truck.Errors.Add("NoPlat", "Tidak boleh diduplikasi");
            }
            return truck;
        }

        public Truck VCreateObject(Truck truck, ITruckService _truckService)
        {
            VName(truck, _truckService);
            if (!isValid(truck)) { return truck; }
            return truck;
        }

        public Truck VUpdateObject(Truck truck, ITruckService _truckService)
        {
            VObject(truck, _truckService);
            if (!isValid(truck)) { return truck; }
            VName(truck, _truckService);
            if (!isValid(truck)) { return truck; }
            return truck;
        }

        public bool isValid(Truck obj)
        { 
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public Truck VObject(Truck truck, ITruckService _truckService)
        {
            Truck oldtruck = _truckService.GetObjectById(truck.Id);
            if (oldtruck == null)
            {
                truck.Errors.Add("Generic", "Invalid Data For Update");
            }
            else if (!VOffice(truck.OfficeId, oldtruck.OfficeId))
            {
                truck.Errors.Add("Generic", "Invalid Data For Update");
            }
            return truck;
        }

        public bool VOffice(int OfficeId, int CekOfficeId)
        {
            return OfficeId == CekOfficeId;
        }

    }
}
