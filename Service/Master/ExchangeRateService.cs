using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ExchangeRateService : IExchangeRateService 
    {  
        private IExchangeRateRepository _repository;
        private IExchangeRateValidation _validator;

        public ExchangeRateService(IExchangeRateRepository _exchangeRateRepository, IExchangeRateValidation _exchangeRateValidation)
        {
            _repository = _exchangeRateRepository;
            _validator = _exchangeRateValidation;
        }

        public IQueryable<ExchangeRate> GetQueryable()
        {  
            return _repository.GetQueryable();
        }

        public ExchangeRate GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public ExchangeRate GetLatestRate(DateTime date)
        {
            return GetQueryable().Where(x => x.ExRateDate <= date).OrderByDescending(x => x.ExRateDate).FirstOrDefault();
        }

        public ExchangeRate CreateObject(ExchangeRate exchangeRate)
        {
            exchangeRate.Errors = new Dictionary<String, String>();
            if (isValid(_validator.VCreateObject(exchangeRate,this)))
            {
                exchangeRate.MasterCode = _repository.GetLastMasterCode(exchangeRate.OfficeId) + 1;
                exchangeRate = _repository.CreateObject(exchangeRate);
            }
            return exchangeRate;
        }
         
        public ExchangeRate UpdateObject(ExchangeRate exchangeRate)
        {
            if (isValid(_validator.VUpdateObject(exchangeRate, this)))
            {
                exchangeRate = _repository.UpdateObject(exchangeRate);
            }
            return exchangeRate;
        }
         
        public ExchangeRate SoftDeleteObject(ExchangeRate exchangeRate)
        {
            exchangeRate = _repository.SoftDeleteObject(exchangeRate);
            return exchangeRate;
        }

        public bool isValid(ExchangeRate obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }
    }
}
