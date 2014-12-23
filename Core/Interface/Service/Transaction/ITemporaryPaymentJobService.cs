using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface ITemporaryPaymentJobService
    {
        IQueryable<TemporaryPaymentJob> GetQueryable();
        TemporaryPaymentJob GetObjectById(int Id);
        TemporaryPaymentJob CreateObject(TemporaryPaymentJob temporaryPaymentJob, ITemporaryPaymentService _temporaryPaymentService);
        TemporaryPaymentJob UpdateObject(TemporaryPaymentJob temporaryPaymentJob); 
        TemporaryPaymentJob SoftDeleteObject(TemporaryPaymentJob temporaryPaymentJob);
    }
}