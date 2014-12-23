using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Validation
{
    public interface ITemporaryPaymentJobValidation
    {
        TemporaryPaymentJob VCreateObject(TemporaryPaymentJob temporaryPaymentJob, ITemporaryPaymentJobService _temporaryPaymentJobService);
        TemporaryPaymentJob VUpdateObject(TemporaryPaymentJob temporaryPaymentJob, ITemporaryPaymentJobService _temporaryPaymentJobService);
    }
}
