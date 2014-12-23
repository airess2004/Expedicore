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
    public class TemporaryPaymentJobService : ITemporaryPaymentJobService 
    {  
        private ITemporaryPaymentJobRepository _repository;
        private ITemporaryPaymentJobValidation _validator;

        public TemporaryPaymentJobService(ITemporaryPaymentJobRepository _temporaryPaymentJobRepository, ITemporaryPaymentJobValidation _temporaryPaymentJobValidation)
        {
            _repository = _temporaryPaymentJobRepository;
            _validator = _temporaryPaymentJobValidation;   
        }

        public IQueryable<TemporaryPaymentJob> GetQueryable()
        {  
            return _repository.GetQueryable();
        }

        public TemporaryPaymentJob GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public TemporaryPaymentJob CreateObject(TemporaryPaymentJob temporaryPaymentJob,ITemporaryPaymentService _temporaryPaymentService)
        {
            temporaryPaymentJob.Errors = new Dictionary<String, String>();
            if (isValid(_validator.VCreateObject(temporaryPaymentJob,this)))
            {
                TemporaryPaymentJob newTPJob = new TemporaryPaymentJob();
                newTPJob.TemporaryPaymentId = temporaryPaymentJob.TemporaryPaymentId;
                newTPJob.CashUSD = temporaryPaymentJob.CashUSD;
                newTPJob.CashIDR = temporaryPaymentJob.CashIDR;
                newTPJob.BankUSD = temporaryPaymentJob.BankUSD;
                newTPJob.BankIDR = temporaryPaymentJob.BankIDR;
                newTPJob.ShipmentOrderId = temporaryPaymentJob.ShipmentOrderId;
                newTPJob.OfficeId = temporaryPaymentJob.OfficeId;
                temporaryPaymentJob = _repository.CreateObject(newTPJob);
                CalculateTotalTPJob(temporaryPaymentJob.TemporaryPaymentId,_temporaryPaymentService);
            }
            return temporaryPaymentJob;
        }

        private void CalculateTotalTPJob(int tpId, ITemporaryPaymentService _temporaryPaymentService)
        { 
            TemporaryPayment temporaryPayment = _temporaryPaymentService.GetObjectById(tpId);
            if (temporaryPayment != null)
            {
                IList<TemporaryPaymentJob> tpJob = _repository.GetQueryable().Where(x => x.TemporaryPaymentId == tpId).ToList();
                decimal TotalCashUSD = 0;
                decimal TotalCashIDR = 0;
                decimal TotalBankUSD = 0;
                decimal TotalBankIDR = 0;
                foreach (TemporaryPaymentJob item in tpJob)
                {
                    TotalCashUSD += item.CashUSD.HasValue ? item.CashUSD.Value : 0;
                    TotalCashIDR += item.CashIDR.HasValue ? item.CashIDR.Value : 0;
                    TotalBankUSD += item.BankUSD.HasValue ? item.BankUSD.Value : 0;
                    TotalBankIDR += item.BankIDR.HasValue ? item.BankIDR.Value : 0;
                }
                temporaryPayment.TotalCashUSD = TotalCashUSD;
                temporaryPayment.TotalCashIDR = TotalCashIDR;
                temporaryPayment.TotalBankUSD = TotalBankUSD;
                temporaryPayment.TotalBankIDR = TotalBankIDR;
                _temporaryPaymentService.UpdateObject(temporaryPayment);
            }
        }

        public TemporaryPaymentJob UpdateObject(TemporaryPaymentJob temporaryPaymentJob)
        {
            if (isValid(_validator.VUpdateObject(temporaryPaymentJob, this)))
            {
                temporaryPaymentJob = _repository.UpdateObject(temporaryPaymentJob);
            }
            return temporaryPaymentJob;
        }

        public TemporaryPaymentJob SoftDeleteObject(TemporaryPaymentJob temporaryPaymentJob)
        {
            temporaryPaymentJob = _repository.SoftDeleteObject(temporaryPaymentJob);
            return temporaryPaymentJob;
        }


        public bool isValid(TemporaryPaymentJob obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }
    }
}
