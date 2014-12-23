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
    public class BillOfLadingValidation : IBillOfLadingValidation
    {
         
        public BillOfLading VBillOfLading(BillOfLading billoflading, IBillOfLadingService _billofladingservice)
        {
            BillOfLading countrylocation = _billofladingservice.GetObjectById(billoflading.Id);
            if (countrylocation == null)
            {
                billoflading.Errors.Add("Country", "Tidak boleh kosong");
            }
            else if (!VOffice(billoflading.OfficeId, countrylocation.OfficeId))
            {
                billoflading.Errors.Add("Country", "Invalid Country");
            }
            return billoflading;
        }

 
        public BillOfLading VCreateObject(BillOfLading billoflading, IBillOfLadingService _billofladingService)
        {
            return billoflading;
        }

        public BillOfLading VUpdateObject(BillOfLading billoflading, IBillOfLadingService _billofladingService)
        { 
            
            return billoflading;
        }

        public bool isValid(BillOfLading obj)
        { 
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public BillOfLading VObject(BillOfLading billoflading, IBillOfLadingService _billofladingService)
        {
            BillOfLading oldbilloflading = _billofladingService.GetObjectById(billoflading.Id);
            if (oldbilloflading == null)
            {
                billoflading.Errors.Add("Generic", "Invalid Data For Update");
            }
            else if (!VOffice(billoflading.OfficeId, oldbilloflading.OfficeId))
            {
                billoflading.Errors.Add("Generic", "Invalid Data For Update");
            }
            return billoflading;
        }

        public bool VOffice(int OfficeId, int CekOfficeId)
        {
            return OfficeId == CekOfficeId;
        }
    }
}
