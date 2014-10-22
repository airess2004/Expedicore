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
    public class EstimateProfitLossDetailService : IEstimateProfitLossDetailService 
    {  
        private IEstimateProfitLossDetailRepository _repository;
        private IEstimateProfitLossDetailValidation _validator;

        public EstimateProfitLossDetailService(IEstimateProfitLossDetailRepository _estimateprofitlossdetailRepository, IEstimateProfitLossDetailValidation _estimateprofitlossdetailValidation)
        {
            _repository = _estimateprofitlossdetailRepository;
            _validator = _estimateprofitlossdetailValidation;
        }

        public IQueryable<EstimateProfitLossDetail> GetQueryable()
        {  
            return _repository.GetQueryable();
        }

        public EstimateProfitLossDetail GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public EstimateProfitLossDetail GetObjectByEstimateProfitLossId(int Id)
        {
            return _repository.GetObjectByEstimateProfitLossId(Id);
        }

        public EstimateProfitLossDetail CreateObject(EstimateProfitLossDetail estimateprofitlossdetail)
        { 
            estimateprofitlossdetail.Errors = new Dictionary<String, String>();
            if (!isValid(_validator.VCreateObject(estimateprofitlossdetail,this)))
            {
                EstimateProfitLossDetail newEPLDetail  = new EstimateProfitLossDetail();
                newEPLDetail.CostId = estimateprofitlossdetail.CostId.Value;
                newEPLDetail.AmountCrr = estimateprofitlossdetail.AmountCrr.Value;
                newEPLDetail.Amount = estimateprofitlossdetail.Amount;
                newEPLDetail.CodingQuantity = estimateprofitlossdetail.CodingQuantity;
                newEPLDetail.OfficeId = estimateprofitlossdetail.OfficeId;
                newEPLDetail.CreatedById = estimateprofitlossdetail.CreatedById;
                newEPLDetail.CreatedAt = DateTime.Now;
                newEPLDetail.CustomerId = estimateprofitlossdetail.CustomerId;
                newEPLDetail.CustumerTypeId = estimateprofitlossdetail.CustumerTypeId;
                newEPLDetail.DataFrom = false;
                newEPLDetail.Description = String.IsNullOrEmpty(estimateprofitlossdetail.Description) ? "" : estimateprofitlossdetail.Description.ToUpper();
                newEPLDetail.EstimateProfitLossId = estimateprofitlossdetail.EstimateProfitLossId;
                newEPLDetail.IsIncome = true;
                newEPLDetail.PerQty = estimateprofitlossdetail.PerQty;
                newEPLDetail.Quantity = estimateprofitlossdetail.Quantity;
                newEPLDetail.Sign = estimateprofitlossdetail.Sign;
                newEPLDetail.Type = estimateprofitlossdetail.Type;


                estimateprofitlossdetail = _repository.CreateObject(newEPLDetail);

            }
            return estimateprofitlossdetail;
        }

     
        public EstimateProfitLossDetail UpdateObject(EstimateProfitLossDetail estimateprofitlossdetail)
        {
            if (!isValid(_validator.VUpdateObject(estimateprofitlossdetail, this)))
            {
                estimateprofitlossdetail = _repository.UpdateObject(estimateprofitlossdetail);
            }
            return estimateprofitlossdetail;
        }
         
        public EstimateProfitLossDetail SoftDeleteObject(EstimateProfitLossDetail estimateprofitlossdetail)
        {
            estimateprofitlossdetail = _repository.SoftDeleteObject(estimateprofitlossdetail);
            return estimateprofitlossdetail;
        }


        public bool isValid(EstimateProfitLossDetail obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }
    }
}
