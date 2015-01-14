using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Validation
{
    public interface ICashSettlementValidation 
    {
        CashSettlement VCreateObject(CashSettlement cashSettlement, ICashSettlementService _cashSettlementService);
        CashSettlement VUpdateObject(CashSettlement cashSettlement, ICashSettlementService _cashSettlementService);
        CashSettlement VConfirmObject(CashSettlement cashSettlement, ICashSettlementService _cashSettlementService);
        CashSettlement VUnconfirmObject(CashSettlement cashSettlement, ICashSettlementService _cashSettlementService);
    }
}
