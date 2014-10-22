using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IEstimateProfitLossService
    {
        IQueryable<EstimateProfitLoss> GetQueryable();
        EstimateProfitLoss GetObjectById(int Id);
        EstimateProfitLoss GetObjectByShipmentOrderId(int Id);
        EstimateProfitLoss CreateUpdateObject(EstimateProfitLoss estimateprofitloss);
        EstimateProfitLoss CreateObject(EstimateProfitLoss estimateprofitloss);
        EstimateProfitLoss UpdateObject(EstimateProfitLoss estimateprofitloss); 
        EstimateProfitLoss SoftDeleteObject(EstimateProfitLoss estimateprofitloss);
    }
}