using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Validation
{
    public interface ICashAdvanceDetailValidation
    {
        CashAdvanceDetail VCreateObject(CashAdvanceDetail cashAdvancedetail, ICashAdvanceDetailService _cashAdvancedetailService);
        CashAdvanceDetail VUpdateObject(CashAdvanceDetail cashAdvancedetail, ICashAdvanceDetailService _cashAdvancedetailService);
        CashAdvanceDetail VConfirmObject(CashAdvanceDetail cashAdvancedetail, ICashAdvanceDetailService _cashAdvancedetailService);
        CashAdvanceDetail VUnconfirmObject(CashAdvanceDetail cashAdvancedetail, ICashAdvanceDetailService _cashAdvancedetailService); 
    }
}
