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
    public class MstBillOfLadingValidation : IMstBillOfLadingValidation
    {  
        public MstBillOfLading VName(MstBillOfLading mstbilloflading, IMstBillOfLadingService _mstbillofladingService)
        {
            if (String.IsNullOrEmpty(mstbilloflading.Name) || mstbilloflading.Name.Trim() == "")
            {
                mstbilloflading.Errors.Add("Name", "Tidak boleh kosong");
            }
            else if (_mstbillofladingService.IsNameDuplicated(mstbilloflading))
            {
                mstbilloflading.Errors.Add("Name", "Tidak boleh diduplikasi");
            }
            return mstbilloflading;
        }

        public MstBillOfLading VAbbrevation(MstBillOfLading mstbilloflading, IMstBillOfLadingService _mstbillofladingService)
        {
            if (String.IsNullOrEmpty(mstbilloflading.Abbrevation) || mstbilloflading.Abbrevation.Trim() == "")
            {
                mstbilloflading.Errors.Add("Abbrevation", "Tidak boleh kosong");
            }
            return mstbilloflading;
        }
        
        public MstBillOfLading VCreateObject(MstBillOfLading mstbilloflading, IMstBillOfLadingService _mstbillofladingService)
        {
            VName(mstbilloflading, _mstbillofladingService);
            if (!isValid(mstbilloflading)) { return mstbilloflading; }
            VAbbrevation(mstbilloflading, _mstbillofladingService);
            if (!isValid(mstbilloflading)) { return mstbilloflading; }
            return mstbilloflading;
        }

        public MstBillOfLading VUpdateObject(MstBillOfLading mstbilloflading, IMstBillOfLadingService _mstbillofladingService)
        { 
            VObject(mstbilloflading, _mstbillofladingService);
            if (!isValid(mstbilloflading)) { return mstbilloflading; }
            VName(mstbilloflading, _mstbillofladingService);
            if (!isValid(mstbilloflading)) { return mstbilloflading; }
            VAbbrevation(mstbilloflading, _mstbillofladingService);
            if (!isValid(mstbilloflading)) { return mstbilloflading; }
            return mstbilloflading;
        }

        public bool isValid(MstBillOfLading obj)
        { 
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public MstBillOfLading VObject(MstBillOfLading mstbilloflading, IMstBillOfLadingService _mstbillofladingService)
        {
            MstBillOfLading oldmstbilloflading = _mstbillofladingService.GetObjectById(mstbilloflading.Id);
            if (oldmstbilloflading == null)
            {
                mstbilloflading.Errors.Add("Generic", "Invalid Data For Update");
            }
            else if (!VOffice(mstbilloflading.OfficeId, oldmstbilloflading.OfficeId))
            {
                mstbilloflading.Errors.Add("Generic", "Invalid Data For Update");
            }
            return mstbilloflading;
        }

        public bool VOffice(int OfficeId, int CekOfficeId)
        {
            return OfficeId == CekOfficeId;
        }

    }
}
