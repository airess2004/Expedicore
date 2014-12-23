using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Validation
{
    public interface IBillOfLadingValidation
    { 
        BillOfLading VCreateObject(BillOfLading billoflading, IBillOfLadingService _billofladingService);
        BillOfLading VUpdateObject(BillOfLading billoflading, IBillOfLadingService _billofladingService);
    }
}
