using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validation.Validation
{ 
    public class ExchangeRateValidation : IExchangeRateValidation
    {  
        
        public ExchangeRate VCreateObject(ExchangeRate exchangeRate, IExchangeRateService _exchangeRateService)
        {
            return exchangeRate;
        }

        public ExchangeRate VUpdateObject(ExchangeRate exchangeRate, IExchangeRateService _exchangeRateService)
        {
            VObject(exchangeRate, _exchangeRateService);
            if (!isValid(exchangeRate)) { return exchangeRate; }
            return exchangeRate;
        }
       
        public ExchangeRate VObject(ExchangeRate exchangeRate, IExchangeRateService _exchangeRateService)
        {
            ExchangeRate oldexchangeRate = _exchangeRateService.GetObjectById(exchangeRate.Id);
            if (oldexchangeRate == null)
            {
                exchangeRate.Errors.Add("Generic", "Invalid Data For Update");
            }
            else if (!VOffice(exchangeRate.OfficeId,oldexchangeRate.OfficeId))
            {
                exchangeRate.Errors.Add("Generic", "Invalid Data For Update");
            }
            return exchangeRate;
        }


        public bool VOffice(int OfficeId, int CekOfficeId)
        {
            return OfficeId == CekOfficeId;
        }

        public bool isValid(ExchangeRate obj)
        { 
            bool isValid = !obj.Errors.Any();
            return isValid;
        }
    }
}
