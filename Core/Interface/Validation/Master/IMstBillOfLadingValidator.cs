using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Validation
{
    public interface IMstBillOfLadingValidation
    {
        MstBillOfLading VCreateObject(MstBillOfLading mstbilloflading, IMstBillOfLadingService _mstbillofladingService);
        MstBillOfLading VUpdateObject(MstBillOfLading mstbilloflading, IMstBillOfLadingService _mstbillofladingService);
    }
}
