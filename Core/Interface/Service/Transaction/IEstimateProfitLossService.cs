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
        EstimateProfitLoss CreateUpdateObject(EstimateProfitLoss estimateprofitloss, IShipmentOrderService _shipmentOrderService);
        EstimateProfitLoss CreateObject(EstimateProfitLoss estimateprofitloss, IShipmentOrderService _shipmentOrderService);
        EstimateProfitLoss UpdateObject(EstimateProfitLoss estimateprofitloss, IShipmentOrderService _shipmentOrderService);
        EstimateProfitLoss SoftDeleteObject(EstimateProfitLoss estimateprofitloss, IEstimateProfitLossDetailService _estimateProfitLossDetailService);
    }
}