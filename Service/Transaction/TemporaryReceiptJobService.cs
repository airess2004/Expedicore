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
    public class TemporaryReceiptJobService : ITemporaryReceiptJobService 
    {  
        private ITemporaryReceiptJobRepository _repository;
        private ITemporaryReceiptJobValidation _validator;

        public TemporaryReceiptJobService(ITemporaryReceiptJobRepository _temporaryReceiptJobRepository, ITemporaryReceiptJobValidation _temporaryReceiptJobValidation)
        {
            _repository = _temporaryReceiptJobRepository;
            _validator = _temporaryReceiptJobValidation;   
        }

        public IQueryable<TemporaryReceiptJob> GetQueryable()
        {  
            return _repository.GetQueryable();
        }

        public TemporaryReceiptJob GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public TemporaryReceiptJob CreateObject(TemporaryReceiptJob temporaryReceiptJob,ITemporaryReceiptService _temporaryReceiptService)
        {
            temporaryReceiptJob.Errors = new Dictionary<String, String>();
            if (isValid(_validator.VCreateObject(temporaryReceiptJob,this)))
            {
                TemporaryReceiptJob newTPJob = new TemporaryReceiptJob();
                newTPJob.TemporaryReceiptId = temporaryReceiptJob.TemporaryReceiptId;
                newTPJob.CashUSD = temporaryReceiptJob.CashUSD;
                newTPJob.CashIDR = temporaryReceiptJob.CashIDR;
                newTPJob.BankUSD = temporaryReceiptJob.BankUSD;
                newTPJob.BankIDR = temporaryReceiptJob.BankIDR;
                newTPJob.ShipmentOrderId = temporaryReceiptJob.ShipmentOrderId;
                newTPJob.OfficeId = temporaryReceiptJob.OfficeId;
                temporaryReceiptJob = _repository.CreateObject(newTPJob);
                CalculateTotalTRJob(temporaryReceiptJob.TemporaryReceiptId,_temporaryReceiptService);
            }
            return temporaryReceiptJob;
        }

        private void CalculateTotalTRJob(int tpId, ITemporaryReceiptService _temporaryReceiptService)
        { 
            TemporaryReceipt temporaryReceipt = _temporaryReceiptService.GetObjectById(tpId);
            if (temporaryReceipt != null)
            {
                IList<TemporaryReceiptJob> tpJob = _repository.GetQueryable().Where(x => x.TemporaryReceiptId == tpId).ToList();
                decimal TotalCashUSD = 0;
                decimal TotalCashIDR = 0;
                decimal TotalBankUSD = 0;
                decimal TotalBankIDR = 0;
                foreach (TemporaryReceiptJob item in tpJob)
                {
                    TotalCashUSD += item.CashUSD.HasValue ? item.CashUSD.Value : 0;
                    TotalCashIDR += item.CashIDR.HasValue ? item.CashIDR.Value : 0;
                    TotalBankUSD += item.BankUSD.HasValue ? item.BankUSD.Value : 0;
                    TotalBankIDR += item.BankIDR.HasValue ? item.BankIDR.Value : 0;
                }
                temporaryReceipt.TotalCashUSD = TotalCashUSD;
                temporaryReceipt.TotalCashIDR = TotalCashIDR;
                temporaryReceipt.TotalBankUSD = TotalBankUSD;
                temporaryReceipt.TotalBankIDR = TotalBankIDR;
                _temporaryReceiptService.UpdateObject(temporaryReceipt);
            }
        }

        public TemporaryReceiptJob UpdateObject(TemporaryReceiptJob temporaryReceiptJob)
        {
            if (isValid(_validator.VUpdateObject(temporaryReceiptJob, this)))
            {
                temporaryReceiptJob = _repository.UpdateObject(temporaryReceiptJob);
            }
            return temporaryReceiptJob;
        }

        public TemporaryReceiptJob SoftDeleteObject(TemporaryReceiptJob temporaryReceiptJob)
        {
            temporaryReceiptJob = _repository.SoftDeleteObject(temporaryReceiptJob);
            return temporaryReceiptJob;
        }


        public bool isValid(TemporaryReceiptJob obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }
    }
}
