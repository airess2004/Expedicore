using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Validation
{
    public interface ICashAdvanceValidation
    {
        CashAdvance VCreateObject(CashAdvance cashAdvance, ICashAdvanceService _cashAdvanceService);
        CashAdvance VUpdateObject(CashAdvance cashAdvance, ICashAdvanceService _cashAdvanceService);
    }
}
