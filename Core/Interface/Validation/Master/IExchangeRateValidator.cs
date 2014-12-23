using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Validation
{
    public interface IExchangeRateValidation
    { 
        ExchangeRate VCreateObject(ExchangeRate exchangerate, IExchangeRateService _exchangerateService);
        ExchangeRate VUpdateObject(ExchangeRate exchangerate, IExchangeRateService _exchangerateService);
    }
}
