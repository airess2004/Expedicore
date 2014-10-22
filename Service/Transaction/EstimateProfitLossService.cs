using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Master
{
    public class EstimateProfitLossService : IEstimateProfitLossService 
    {  
        private IEstimateProfitLossRepository _repository;
        private IEstimateProfitLossValidation _validator;

        public EstimateProfitLossService(IEstimateProfitLossRepository _estimateprofitlossRepository, IEstimateProfitLossValidation _estimateprofitlossValidation)
        {
            _repository = _estimateprofitlossRepository;
            _validator = _estimateprofitlossValidation;
        }

        public IQueryable<EstimateProfitLoss> GetQueryable()
        {  
            return _repository.GetQueryable();
        }

        public EstimateProfitLoss GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public EstimateProfitLoss GetObjectByShipmentOrderId(int Id)
        {
            return _repository.GetObjectByShipmentOrderId(Id);
        }

        public EstimateProfitLoss CreateUpdateObject(EstimateProfitLoss estimateprofitloss)
        {
            EstimateProfitLoss newestimateprofitloss = this.GetObjectByShipmentOrderId(estimateprofitloss.ShipmentOrderId);
            if (newestimateprofitloss == null)
            {
                estimateprofitloss = this.CreateObject(estimateprofitloss);
            }
            else
            {
                estimateprofitloss = this.UpdateObject(estimateprofitloss);
            }
            return estimateprofitloss;
        }

        public EstimateProfitLoss CreateObject(EstimateProfitLoss estimateprofitloss)
        {
            estimateprofitloss.Errors = new Dictionary<String, String>();
            if (!isValid(_validator.VCreateObject(estimateprofitloss,this)))
            {
                EstimateProfitLoss newEPL  = new EstimateProfitLoss();
                newEPL.IsDeleted = false;
                newEPL.OfficeId = estimateprofitloss.OfficeId;
                newEPL.CreatedById = estimateprofitloss.CreatedById;
                newEPL.CreatedAt = DateTime.Today;
                newEPL.EstIDRAgent = estimateprofitloss.EstIDRAgent;
                newEPL.EstIDRShipCons = estimateprofitloss.EstIDRShipCons;
                newEPL.ShipmentOrderId = estimateprofitloss.ShipmentOrderId;
                estimateprofitloss = _repository.CreateObject(newEPL);

            }
            return estimateprofitloss;
        }

     
        public EstimateProfitLoss UpdateObject(EstimateProfitLoss estimateprofitloss)
        {
            if (!isValid(_validator.VUpdateObject(estimateprofitloss, this)))
            {
                estimateprofitloss = _repository.UpdateObject(estimateprofitloss);
            }
            return estimateprofitloss;
        }
         
        public EstimateProfitLoss SoftDeleteObject(EstimateProfitLoss estimateprofitloss)
        {
            estimateprofitloss = _repository.SoftDeleteObject(estimateprofitloss);
            return estimateprofitloss;
        }


        public bool isValid(EstimateProfitLoss obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }
    }
}
