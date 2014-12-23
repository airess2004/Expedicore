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
    public class TemporaryPaymentService : ITemporaryPaymentService 
    {  
        private ITemporaryPaymentRepository _repository;
        private ITemporaryPaymentValidation _validator;

        public TemporaryPaymentService(ITemporaryPaymentRepository _temporarypaymentRepository, ITemporaryPaymentValidation _temporarypaymentValidation)
        {
            _repository = _temporarypaymentRepository;
            _validator = _temporarypaymentValidation;   
        }

        public IQueryable<TemporaryPayment> GetQueryable()
        {  
            return _repository.GetQueryable();
        }

        public TemporaryPayment GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public TemporaryPayment CreateObject(TemporaryPayment temporarypayment,IExchangeRateService _exchangeRateService)
        {
            temporarypayment.Errors = new Dictionary<String, String>();
            if (isValid(_validator.VCreateObject(temporarypayment,this)))
            {
                TemporaryPayment newTP = new TemporaryPayment();
                newTP.CostId = temporarypayment.CostId;
                newTP.OfficeId = temporarypayment.OfficeId;
                newTP.CreatedById =temporarypayment.CreatedById;
                newTP.CreatedAt = DateTime.Today;

                newTP.ContactId = temporarypayment.ContactId;
                newTP.PaymentTo = temporarypayment.PaymentTo;

                newTP.TPDueDate = temporarypayment.TPDueDate;

                newTP.JobCode = temporarypayment.JobCode;
                newTP.JobOwnerId = temporarypayment.JobOwnerId;

                newTP.TotalCashUSD = temporarypayment.TotalCashUSD;
                newTP.TotalCashIDR = temporarypayment.TotalCashIDR;
                newTP.TotalBankUSD = temporarypayment.TotalBankUSD;
                newTP.TotalBankIDR = temporarypayment.TotalBankIDR;
                 
                // Rate Info
                ExchangeRate rateInfo = _exchangeRateService.GetQueryable()
                                .Where(x => x.ExRateDate <= DateTime.Today && x.OfficeId == temporarypayment.OfficeId)
                                .OrderByDescending(x => x.ExRateDate).FirstOrDefault(); ;
                if (rateInfo != null)
                {
                    newTP.ExRateId = rateInfo.Id;
                    newTP.ExRateDate = rateInfo.ExRateDate;
                    newTP.Rate = rateInfo.ExRate1;
                }
                newTP.TPNo = _repository.GetLastTPNo(temporarypayment.OfficeId) + 1;
                temporarypayment = _repository.CreateObject(newTP);
            }
            return temporarypayment;
        }
         
        public TemporaryPayment UpdateObject(TemporaryPayment temporarypayment)
        {
            if (isValid(_validator.VUpdateObject(temporarypayment, this)))
            {
                temporarypayment = _repository.UpdateObject(temporarypayment);
            }
            return temporarypayment;
        }

        public TemporaryPayment SoftDeleteObject(TemporaryPayment temporarypayment)
        {
            temporarypayment = _repository.SoftDeleteObject(temporarypayment);
            return temporarypayment;
        }


        public bool isValid(TemporaryPayment obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }
    }
}
