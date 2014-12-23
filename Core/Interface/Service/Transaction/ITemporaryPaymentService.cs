using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface ITemporaryPaymentService
    {
        IQueryable<TemporaryPayment> GetQueryable();
        TemporaryPayment GetObjectById(int Id);
        TemporaryPayment CreateObject(TemporaryPayment temporarypayment, IExchangeRateService _exchangeRateService);
        TemporaryPayment UpdateObject(TemporaryPayment temporarypayment); 
        TemporaryPayment SoftDeleteObject(TemporaryPayment temporarypayment);
    }
}