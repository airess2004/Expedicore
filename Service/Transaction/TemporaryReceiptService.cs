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
    public class TemporaryReceiptService : ITemporaryReceiptService 
    {  
        private ITemporaryReceiptRepository _repository;
        private ITemporaryReceiptValidation _validator;

        public TemporaryReceiptService(ITemporaryReceiptRepository _temporaryPaymentRepository, ITemporaryReceiptValidation _temporaryPaymentValidation)
        {
            _repository = _temporaryPaymentRepository;
            _validator = _temporaryPaymentValidation;   
        }

        public IQueryable<TemporaryReceipt> GetQueryable()
        {  
            return _repository.GetQueryable();
        }

        public TemporaryReceipt GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public TemporaryReceipt CreateObject(TemporaryReceipt temporaryPayment,IExchangeRateService _exchangeRateService)
        {
            temporaryPayment.Errors = new Dictionary<String, String>();
            if (isValid(_validator.VCreateObject(temporaryPayment,this)))
            {
                TemporaryReceipt newTR = new TemporaryReceipt();
                newTR.CostId = temporaryPayment.CostId;
                newTR.OfficeId = temporaryPayment.OfficeId;
                newTR.CreatedById =temporaryPayment.CreatedById;
                newTR.CreatedAt = DateTime.Today;

                newTR.ContactId = temporaryPayment.ContactId;

                newTR.TRDueDate = temporaryPayment.TRDueDate;

                newTR.JobCode = temporaryPayment.JobCode;
                newTR.JobOwnerId = temporaryPayment.JobOwnerId;

                newTR.TotalCashUSD = temporaryPayment.TotalCashUSD;
                newTR.TotalCashIDR = temporaryPayment.TotalCashIDR;
                newTR.TotalBankUSD = temporaryPayment.TotalBankUSD;
                newTR.TotalBankIDR = temporaryPayment.TotalBankIDR;
                 
                // Rate Info
                ExchangeRate rateInfo = _exchangeRateService.GetQueryable()
                                .Where(x => x.ExRateDate <= DateTime.Today && x.OfficeId == temporaryPayment.OfficeId)
                                .OrderByDescending(x => x.ExRateDate).FirstOrDefault(); ;
                if (rateInfo != null)
                {
                    newTR.ExRateId = rateInfo.Id;
                    newTR.ExRateDate = rateInfo.ExRateDate;
                    newTR.Rate = rateInfo.ExRate1;
                }
                newTR.TemporaryReceiptNo = _repository.GetLastTRNo(temporaryPayment.OfficeId) + 1;
                temporaryPayment = _repository.CreateObject(newTR);
            }
            return temporaryPayment;
        }
         
        public TemporaryReceipt UpdateObject(TemporaryReceipt temporaryPayment)
        {
            if (isValid(_validator.VUpdateObject(temporaryPayment, this)))
            {
                temporaryPayment = _repository.UpdateObject(temporaryPayment);
            }
            return temporaryPayment;
        }

        public TemporaryReceipt SoftDeleteObject(TemporaryReceipt temporaryPayment)
        {
            temporaryPayment = _repository.SoftDeleteObject(temporaryPayment);
            return temporaryPayment;
        }


        public bool isValid(TemporaryReceipt obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }
    }
}
