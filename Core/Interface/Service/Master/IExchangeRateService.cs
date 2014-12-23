using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IExchangeRateService
    {
        IQueryable<ExchangeRate> GetQueryable();
        ExchangeRate GetObjectById(int Id);
        ExchangeRate CreateObject(ExchangeRate exchangerate);
        ExchangeRate UpdateObject(ExchangeRate exchangerate);
        ExchangeRate GetLatestRate(DateTime date);
    }
}