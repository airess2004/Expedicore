using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Validation
{
    public interface ITemporaryPaymentValidation
    {
        TemporaryPayment VCreateObject(TemporaryPayment temporarypayment, ITemporaryPaymentService _temporarypaymentService);
        TemporaryPayment VUpdateObject(TemporaryPayment temporarypayment, ITemporaryPaymentService _temporarypaymentService);
    }
}
