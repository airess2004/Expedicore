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
    public class TruckOrderValidation : ITruckOrderValidation
    {  
        public TruckOrder VName(TruckOrder truckOrder, ITruckOrderService _truckOrderService)
        {
            if (String.IsNullOrEmpty(truckOrder.NoJob) || truckOrder.NoJob.Trim() == "")
            {
                truckOrder.Errors.Add("NoJob", "Tidak boleh kosong");
            }
            return truckOrder;
        }

        public TruckOrder VCreateObject(TruckOrder truckOrder, ITruckOrderService _truckOrderService)
        {
            VName(truckOrder, _truckOrderService);
            if (!isValid(truckOrder)) { return truckOrder; }
            return truckOrder;
        }

        public TruckOrder VUpdateObject(TruckOrder truckOrder, ITruckOrderService _truckOrderService)
        {
            VObject(truckOrder, _truckOrderService);
            if (!isValid(truckOrder)) { return truckOrder; }
            VName(truckOrder, _truckOrderService);
            if (!isValid(truckOrder)) { return truckOrder; }
            return truckOrder;
        }

        public bool isValid(TruckOrder obj)
        { 
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public TruckOrder VObject(TruckOrder truckOrder, ITruckOrderService _truckOrderService)
        {
            TruckOrder oldtruckOrder = _truckOrderService.GetObjectById(truckOrder.Id);
            if (oldtruckOrder == null)
            {
                truckOrder.Errors.Add("Generic", "Invalid Data For Update");
            }
            else if (!VOffice(truckOrder.OfficeId, oldtruckOrder.OfficeId))
            {
                truckOrder.Errors.Add("Generic", "Invalid Data For Update");
            }
            return truckOrder;
        }

        public bool VOffice(int OfficeId, int CekOfficeId)
        {
            return OfficeId == CekOfficeId;
        }

    }
}
