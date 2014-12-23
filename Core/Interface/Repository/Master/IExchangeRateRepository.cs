using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IExchangeRateRepository : IRepository<ExchangeRate>
    {
       IQueryable<ExchangeRate> GetQueryable();
       ExchangeRate GetObjectById(int Id);
       ExchangeRate CreateObject(ExchangeRate model);
       ExchangeRate UpdateObject(ExchangeRate model);
       ExchangeRate SoftDeleteObject(ExchangeRate model);
       bool DeleteObject(int Id);
       int GetLastMasterCode(int officeId);
    }
}